﻿using APIBase.Core.DAO.Models;
using APIBase.Core.DAO.Repositories;
using APIBase.Core.DAO.SerializationErrors;
using APIBase.Core.Tools;
using APIBase.DAO.Tools;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace APIBase.DAO.Repositories
{
    /// <summary>
    /// Context-based repository with synchronous and asynchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    internal class MemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class, IGuidResolvable, IValidatable
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="entityTypes">The entity types of the model</param>
        /// <exception cref="TypeLoadException">Occurs when TEntity is not a set type registered in the context</exception>
        public MemoryRepository(IEnumerable<IEntityType> entityTypes)
        {
            Set = new List<TEntity>();
            RequiredProperties = new ReadOnlyCollection<PropertyInfo>(typeof(TEntity).GetProperties().Where((property) => Attribute.IsDefined(property, typeof(RequiredAttribute))).ToList());
            UniqueIndexesQueryBuilder = new(entityTypes);
        }

        /// <summary>
        /// Gets or sets the required properties of the type of entity managed by the repository.
        /// </summary>
        protected virtual IReadOnlyCollection<PropertyInfo> RequiredProperties { get; init; }

        /// <summary>
        /// Gets or sets the context set used by the repository (operations are performed on it).
        /// </summary>
        protected virtual IList<TEntity> Set { get; init; }

        /// <summary>
        /// Gets or sets the query builder to check the integrity of unique indexes on entities.
        /// </summary>
        protected virtual UniqueIndexesQueryBuilder<TEntity> UniqueIndexesQueryBuilder { get; init; }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Add(TEntity)"/>
        public void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.AddAsync(TEntity)"/>
        public Task AddAsync(TEntity entity)
        {
            Add(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.AddRange(IEnumerable{TEntity})"/>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.AddRangeAsync(IEnumerable{TEntity})"/>
        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            AddRange(entities);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanDelete(TEntity)"/>
        public bool CanDelete(TEntity entity)
        {
            if (!entity.CanBeDeleted())
            {
                return false;
            }
            if (!IsInDatabase(entity))
            {
                entity.SetSerializationResultOnError(new NotFoundInDatabase());
                return false;
            }
            entity.SetSerializationResultOnSuccess();
            return true;
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanDeleteAsync(TEntity)"/>
        public async Task<bool> CanDeleteAsync(TEntity entity)
        {
            if (!entity.CanBeDeleted())
            {
                return false;
            }
            if (!await IsInDatabaseAsync(entity))
            {
                entity.SetSerializationResultOnError(new NotFoundInDatabase());
                return false;
            }
            entity.SetSerializationResultOnSuccess();
            return true;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanDeleteRange(IEnumerable{TEntity})"/>
        public (bool, TEntity) CanDeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (!entity.CanBeDeleted())
                {
                    return (false, entity);
                }
            }
            if (IsAnyNotInDatabase(entities) is TEntity notFoundEntity)
            {
                notFoundEntity.SetSerializationResultOnError(new NotFoundInDatabase());
                return (false, notFoundEntity);
            }
            return (true, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanDeleteRangeAsync(IEnumerable{TEntity})"/>
        public async Task<(bool, TEntity)> CanDeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (!entity.CanBeDeleted())
                {
                    return (false, entity);
                }
            }
            if (await IsAnyNotInDatabaseAsync(entities) is TEntity notFoundEntity)
            {
                notFoundEntity.SetSerializationResultOnError(new NotFoundInDatabase());
                return (false, notFoundEntity);
            }
            return (true, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanSave(TEntity)"/>
        public bool CanSave(TEntity entity)
        {
            if (!entity.CanBeSavedOrUpdated())
            {
                return false;
            }
            if (IsInDatabase(entity))
            {
                entity.SetSerializationResultOnError(new AlreadyExistsInDatabase());
                return false;
            }
            if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
            {
                entity.SetSerializationResultOnError(requiredError);
                return false;
            }
            if (AreUniqueIndexesRespected(entity, null) is SerializationError indexError)
            {
                entity.SetSerializationResultOnError(indexError);
                return false;
            }
            entity.SetSerializationResultOnSuccess();
            return true;
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanSaveAsync(TEntity)"/>
        public async Task<bool> CanSaveAsync(TEntity entity)
        {
            if (!entity.CanBeSavedOrUpdated())
            {
                return false;
            }
            if (await IsInDatabaseAsync(entity))
            {
                entity.SetSerializationResultOnError(new AlreadyExistsInDatabase());
                return false;
            }
            if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
            {
                entity.SetSerializationResultOnError(requiredError);
                return false;
            }
            if (await AreUniqueIndexesRespectedAsync(entity, null) is SerializationError indexError)
            {
                entity.SetSerializationResultOnError(indexError);
                return false;
            }
            entity.SetSerializationResultOnSuccess();
            return true;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanSaveRange(IEnumerable{TEntity})"/>
        public (bool, TEntity) CanSaveRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (!entity.CanBeSavedOrUpdated())
                {
                    return (false, entity);
                }
            }
            if (IsAnyInDatabase(entities) is TEntity existingEntity)
            {
                existingEntity.SetSerializationResultOnError(new AlreadyExistsInDatabase());
                return (false, existingEntity);
            }
            foreach (TEntity entity in entities)
            {
                if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
                {
                    entity.SetSerializationResultOnError(requiredError);
                    return (false, entity);
                }
            }
            foreach (TEntity entity in entities)
            {
                if (AreUniqueIndexesRespected(entity, entities) is SerializationError indexError)
                {
                    entity.SetSerializationResultOnError(indexError);
                    return (false, entity);
                }
            }
            return (true, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanSaveRangeAsync(IEnumerable{TEntity})"/>
        public async Task<(bool, TEntity)> CanSaveRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (!entity.CanBeSavedOrUpdated())
                {
                    return (false, entity);
                }
            }
            if (await IsAnyInDatabaseAsync(entities) is TEntity existingEntity)
            {
                existingEntity.SetSerializationResultOnError(new AlreadyExistsInDatabase());
                return (false, existingEntity);
            }
            foreach (TEntity entity in entities)
            {
                if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
                {
                    entity.SetSerializationResultOnError(requiredError);
                    return (false, entity);
                }
            }
            foreach (TEntity entity in entities)
            {
                if (await AreUniqueIndexesRespectedAsync(entity, entities) is SerializationError indexError)
                {
                    entity.SetSerializationResultOnError(indexError);
                    return (false, entity);
                }
            }
            return (true, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanUpdate(TEntity)"/>
        public bool CanUpdate(TEntity entity)
        {
            if (!entity.CanBeSavedOrUpdated())
            {
                return false;
            }
            if (!IsInDatabase(entity))
            {
                entity.SetSerializationResultOnError(new NotFoundInDatabase());
                return false;
            }
            if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
            {
                entity.SetSerializationResultOnError(requiredError);
                return false;
            }
            if (AreUniqueIndexesRespected(entity, null) is SerializationError indexError)
            {
                entity.SetSerializationResultOnError(indexError);
                return false;
            }
            entity.SetSerializationResultOnSuccess();
            return true;
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanUpdateAsync(TEntity)"/>
        public async Task<bool> CanUpdateAsync(TEntity entity)
        {
            if (!entity.CanBeSavedOrUpdated())
            {
                return false;
            }
            if (!await IsInDatabaseAsync(entity))
            {
                entity.SetSerializationResultOnError(new NotFoundInDatabase());
                return false;
            }
            if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
            {
                entity.SetSerializationResultOnError(requiredError);
                return false;
            }
            if (await AreUniqueIndexesRespectedAsync(entity, null) is SerializationError indexError)
            {
                entity.SetSerializationResultOnError(indexError);
                return false;
            }
            entity.SetSerializationResultOnSuccess();
            return true;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanUpdateRange(IEnumerable{TEntity})"/>
        public (bool, TEntity) CanUpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (!entity.CanBeSavedOrUpdated())
                {
                    return (false, entity);
                }
            }
            if (IsAnyNotInDatabase(entities) is TEntity notFoundEntity)
            {
                notFoundEntity.SetSerializationResultOnError(new NotFoundInDatabase());
                return (false, notFoundEntity);
            }
            foreach (TEntity entity in entities)
            {
                if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
                {
                    entity.SetSerializationResultOnError(requiredError);
                    return (false, entity);
                }
            }
            foreach (TEntity entity in entities)
            {
                if (AreUniqueIndexesRespected(entity, entities) is SerializationError indexError)
                {
                    entity.SetSerializationResultOnError(indexError);
                    return (false, entity);
                }
            }
            return (true, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanUpdateRangeAsync(IEnumerable{TEntity})"/>
        public async Task<(bool, TEntity)> CanUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (!entity.CanBeSavedOrUpdated())
                {
                    return (false, entity);
                }
            }
            if (await IsAnyNotInDatabaseAsync(entities) is TEntity notFoundEntity)
            {
                notFoundEntity.SetSerializationResultOnError(new NotFoundInDatabase());
                return (false, notFoundEntity);
            }
            foreach (TEntity entity in entities)
            {
                if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
                {
                    entity.SetSerializationResultOnError(requiredError);
                    return (false, entity);
                }
            }
            foreach (TEntity entity in entities)
            {
                if (await AreUniqueIndexesRespectedAsync(entity, entities) is SerializationError indexError)
                {
                    entity.SetSerializationResultOnError(indexError);
                    return (false, entity);
                }
            }
            return (true, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Delete(TEntity)"/>
        public void Delete(TEntity entity)
        {
            Set.Remove(entity);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.DeleteAsync(TEntity)"/>
        public Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.DeleteRange(IEnumerable{TEntity})"/>
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.DeleteRangeAsync(IEnumerable{TEntity})"/>
        public Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            DeleteRange(entities);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Find(Expression{Func{TEntity, bool}})"/>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Where(predicate.Compile()).ToList();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindAll"/>
        public IEnumerable<TEntity> FindAll()
        {
            return Set.ToList();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindAllAsync"/>
        public Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return Task.FromResult(FindAll());
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindAsync(Expression{Func{TEntity, bool}})"/>
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Find(predicate));
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindOne(Guid)"/>
        public TEntity FindOne(Guid id)
        {
            return Set.Where(entity => entity.Id == id).FirstOrDefault();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindOne(Expression{Func{TEntity, bool}})"/>
        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Where(predicate.Compile()).FirstOrDefault();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindOneAsync(Guid)"/>
        public Task<TEntity> FindOneAsync(Guid id)
        {
            return Task.FromResult(FindOne(id));
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindOneAsync(Expression{Func{TEntity, bool}})"/>
        public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FindOne(predicate));
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsAnyInDatabase(IEnumerable{TEntity})"/>
        public TEntity IsAnyInDatabase(IEnumerable<TEntity> entities)
        {
            return entities.Where(entity => entity.Id != Guid.Empty).Union(Set).FirstOrDefault();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsAnyInDatabaseAsync(IEnumerable{TEntity})"/>
        public Task<TEntity> IsAnyInDatabaseAsync(IEnumerable<TEntity> entities)
        {
            return Task.FromResult(IsAnyInDatabase(entities));
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsAnyNotInDatabase(IEnumerable{TEntity})"/>
        public TEntity IsAnyNotInDatabase(IEnumerable<TEntity> entities)
        {
            return entities.Except(Set).FirstOrDefault();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsAnyNotInDatabaseAsync(IEnumerable{TEntity})"/>
        public Task<TEntity> IsAnyNotInDatabaseAsync(IEnumerable<TEntity> entities)
        {
            return Task.FromResult(IsAnyNotInDatabase(entities));
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsInDatabase(TEntity)"/>
        public bool IsInDatabase(TEntity entity)
        {
            return entity.Id != Guid.Empty && Set.Any((other) => entity.Id == other.Id);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsInDatabaseAsync(TEntity)"/>
        public Task<bool> IsInDatabaseAsync(TEntity entity)
        {
            return Task.FromResult(IsInDatabase(entity));
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Rollback(TEntity)"/>
        public void Rollback(TEntity entity)
        {
            throw new NotSupportedException("Rollback an object is only possible in a context-based repository");
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.RollbackAsync(TEntity)"/>
        public Task RollbackAsync(TEntity entity)
        {
            Rollback(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.RollbackRange(IEnumerable{TEntity})"/>
        public void RollbackRange(IEnumerable<TEntity> entities)
        {
            throw new NotSupportedException("Rollback objects is only possible in a context-based repository");
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.RollbackRangeAsync(IEnumerable{TEntity})"/>
        public Task RollbackRangeAsync(IEnumerable<TEntity> entities)
        {
            RollbackRange(entities);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.SaveUpdate(TEntity)"/>
        public void SaveUpdate(TEntity entity)
        {
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.SaveUpdateAsync(TEntity)"/>
        public Task SaveUpdateAsync(TEntity entity)
        {
            SaveUpdate(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.SaveUpdateRange(IEnumerable{TEntity})"/>
        public void SaveUpdateRange(IEnumerable<TEntity> entities)
        {
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.SaveUpdateRangeAsync(IEnumerable{TEntity})"/>
        public Task SaveUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            SaveUpdateRange(entities);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if the integrity of the required attributes (not null) of an entity is respected.
        /// </summary>
        /// <param name="entity">The entity to be checked</param>
        /// <returns>The error if the integrity is not respected, or null if there is no problem</returns>
        protected SerializationError AreRequiredPropertiesFilled(TEntity entity)
        {
            foreach (PropertyInfo property in RequiredProperties)
            {
                if (property.GetValue(entity) is null)
                {
                    return new RequiredAttributeViolated(property.Name);
                }
            }
            return null;
        }

        /// <summary>
        /// Checks synchronously if the integrity of unique indexes is respected on an entity (with an entity set).
        /// </summary>
        /// <param name="entity">The entity to be checked</param>
        /// <param name="entitySet">The entity additionnal set to be checked</param>
        /// <returns>The error if the integrity is not respected, or null if there is no problem</returns>
        protected SerializationError AreUniqueIndexesRespected(TEntity entity, IEnumerable<TEntity> entitySet)
        {
            IList<TEntity> toCompare = Set;
            if (entitySet is not null)
            {
                toCompare = toCompare.Concat(entitySet).ToList();
            }
            if (toCompare.Where(UniqueIndexesQueryBuilder.BuildUniqueIndexesExpression(entity).Compile()).Where(item => entity.Id != item.Id).FirstOrDefault() is TEntity conflictItem)
            {
                return new UniqueIndexViolated<Guid>(conflictItem.Id);
            }
            return null;
        }

        /// <summary>
        /// Checks asynchronously if the integrity of unique indexes is respected on an entity (with an entity set).
        /// </summary>
        /// <param name="entity">The entity to be checked</param>
        /// <param name="entitySet">The entity set to be checked</param>
        /// <returns>The error if the integrity is not respected, or null if there is no problem</returns>
        protected Task<SerializationError> AreUniqueIndexesRespectedAsync(TEntity entity, IEnumerable<TEntity> entitySet)
        {
            return Task.FromResult(AreUniqueIndexesRespected(entity, entitySet));
        }
    }
}