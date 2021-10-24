using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace APIBase.DAO.Tools
{
    /// <summary>
    /// An expression tree builder which allows to dynamically create Linq queries on objects.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be queried</typeparam>
    internal class UniqueIndexesQueryBuilder<TEntity>
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="context">The context to be used for the object initialization</param>
        /// <exception cref="TypeLoadException">Occurs when TEntity is not in the context model</exception>
        public UniqueIndexesQueryBuilder(DbContext context)
        {
            Dictionary<IEntityType, IReadOnlyCollection<PropertyInfo>> entityTypesAndPrimaryKeys = new();
            foreach (IEntityType entityType in context.Model.GetEntityTypes())
            {
                List<PropertyInfo> keyProperties = null;
                if (entityType.FindPrimaryKey() is IKey entityKey)
                {
                    keyProperties = new List<PropertyInfo>(entityKey.Properties.Select(keyProperty => keyProperty.PropertyInfo));
                }
                entityTypesAndPrimaryKeys.Add(entityType, new ReadOnlyCollection<PropertyInfo>(keyProperties));
            }
            EntityTypesAndPrimaryKeys = new ReadOnlyDictionary<IEntityType, IReadOnlyCollection<PropertyInfo>>(entityTypesAndPrimaryKeys);
            if (!EntityTypesAndPrimaryKeys.Keys.Select(entityType => entityType.ClrType).Contains(typeof(TEntity)))
            {
                throw new TypeLoadException($"The type {typeof(TEntity)} is not in the database model");
            }
            Dictionary<IEntityType, IReadOnlyCollection<PropertyInfo>> ownedTypesProperties = new();
            foreach (IEntityType ownedType in EntityTypesAndPrimaryKeys.Keys.Where(entityType => entityType.IsOwned()))
            {
                ownedTypesProperties.Add(ownedType, new ReadOnlyCollection<PropertyInfo>(ownedType.ClrType.GetProperties().Where(prop => !prop.GetCustomAttributes<NotMappedAttribute>().Any()).ToList()));
            }
            OwnedTypesProperties = new ReadOnlyDictionary<IEntityType, IReadOnlyCollection<PropertyInfo>>(ownedTypesProperties);
            ParameterExpression = Expression.Parameter(typeof(TEntity));
            Dictionary<IndexAttribute, IReadOnlyCollection<PropertyInfo>> uniqueIndexesProperties = new();
            foreach (IndexAttribute attribute in Attribute.GetCustomAttributes(typeof(TEntity), typeof(IndexAttribute)).Cast<IndexAttribute>().Where(attribute => attribute.IsUnique).ToList())
            {
                uniqueIndexesProperties.Add(attribute, new ReadOnlyCollection<PropertyInfo>(typeof(TEntity).GetProperties().Where(prop => !prop.GetCustomAttributes<NotMappedAttribute>().Any() && attribute.PropertyNames.Contains(prop.Name)).ToList()));
            }
            UniqueIndexesProperties = new ReadOnlyDictionary<IndexAttribute, IReadOnlyCollection<PropertyInfo>>(uniqueIndexesProperties);
        }

        /// <summary>
        /// Gets or sets entity types and all primary keys for each of them.
        /// </summary>
        protected IReadOnlyDictionary<IEntityType, IReadOnlyCollection<PropertyInfo>> EntityTypesAndPrimaryKeys { get; init; }

        /// <summary>
        /// Gets or sets all owned types and all their properties for each of them.
        /// </summary>
        protected IReadOnlyDictionary<IEntityType, IReadOnlyCollection<PropertyInfo>> OwnedTypesProperties { get; init; }

        /// <summary>
        /// Gets or sets a parameter expression for the queries.
        /// </summary>
        protected ParameterExpression ParameterExpression { get; init; }

        /// <summary>
        /// Gets or sets all unique indexes of the TEntity class and the involved properties for each of them.
        /// </summary>
        protected IReadOnlyDictionary<IndexAttribute, IReadOnlyCollection<PropertyInfo>> UniqueIndexesProperties { get; init; }

        /// <summary>
        /// Builds an expression that constitutes a Linq query verifying that the unique indexes of an entity are respected (by checking the properties involved).
        /// </summary>
        /// <param name="entity">The entity to be checked</param>
        /// <returns>The constructed expression</returns>
        public Expression<Func<TEntity, bool>> BuildUniqueIndexesExpression(TEntity entity)
        {
            List<Expression> indexesExpressions = new();
            foreach (KeyValuePair<IndexAttribute, IReadOnlyCollection<PropertyInfo>> pair in UniqueIndexesProperties)
            {
                List<Expression> indexPropertiesExpressions = new();
                foreach (PropertyInfo property in pair.Value)
                {
                    indexPropertiesExpressions.Add(BuildPropertyExpressionNode(property, Expression.Constant(entity), ParameterExpression));
                }
                indexesExpressions.Add(indexPropertiesExpressions.Aggregate((previous, current) => Expression.And(previous, current)));
            }
            return Expression.Lambda<Func<TEntity, bool>>(indexesExpressions.Aggregate((previous, current) => Expression.Or(previous, current)), ParameterExpression);
        }

        /// <summary>
        /// Builds a sub-node of an expression recursively.
        /// </summary>
        /// <param name="property">The current property (the one to be built)</param>
        /// <param name="constantBase">The constant expression parameter</param>
        /// <param name="parameterBase">The variable expression parameter</param>
        /// <returns>The constructed expression for the sub-node</returns>
        /// <exception cref="FormatException">Occurs when a unique index include a property with a keyless type, a collection type, or an object typeo outside the object model types</exception>
        protected Expression BuildPropertyExpressionNode(PropertyInfo property, Expression constantBase, Expression parameterBase)
        {
            if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
            {
                return Expression.Equal(Expression.Property(parameterBase, property), Expression.Property(constantBase, property));
            }
            if (EntityTypesAndPrimaryKeys.Keys.Where(entityType => entityType.ClrType == property.PropertyType).FirstOrDefault() is IEntityType propertyEntityType)
            {
                if (propertyEntityType.IsOwned())
                {
                    return BuildSubPropertiesExpressionNode(property, OwnedTypesProperties[propertyEntityType], constantBase, parameterBase);
                }
                else
                {
                    if (EntityTypesAndPrimaryKeys[propertyEntityType] is IEnumerable<PropertyInfo> keyProperties)
                    {
                        return BuildSubPropertiesExpressionNode(property, keyProperties, constantBase, parameterBase);
                    }
                    else
                    {
                        throw new FormatException($"The type {property.PropertyType} can not be in a unique index of {typeof(TEntity)} because it's a keyless type");
                    }
                }
            }
            if (property.PropertyType.IsSubclassOf(typeof(IEnumerable)))
            {
                throw new FormatException($"The type {property.PropertyType} can not be in a unique index of {typeof(TEntity)} because no collection types are allowed");
            }
            throw new FormatException($"The object type {property.PropertyType} can not be in a unique index of {typeof(TEntity)} because no object types outside the object model types are allowed");
        }

        /// <summary>
        /// Builds the expressions of the sub-properties of a property and aggregates them into a single expression.
        /// </summary>
        /// <param name="baseProperty">The base property</param>
        /// <param name="subProperties">The sub-properties of the base property</param>
        /// <param name="constantBase">The constant expression parameter</param>
        /// <param name="parameterBase">The variable expression parameter</param>
        /// <returns></returns>
        protected Expression BuildSubPropertiesExpressionNode(PropertyInfo baseProperty, IEnumerable<PropertyInfo> subProperties, Expression constantBase, Expression parameterBase)
        {
            List<Expression> subExpressions = new();
            foreach (PropertyInfo subProperty in subProperties)
            {
                subExpressions.Add(BuildPropertyExpressionNode(subProperty, Expression.Property(constantBase, baseProperty), Expression.Property(parameterBase, baseProperty)));
            }
            return subExpressions.Aggregate((previous, current) => Expression.And(previous, current));
        }
    }
}