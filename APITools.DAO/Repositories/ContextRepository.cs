using APITools.Core.DAO.Models;
using APITools.Core.DAO.SerializationErrors;
using APITools.Core.Server.DAO.Repositories;
using APITools.DAO.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace APITools.DAO.Repositories
{
    /// <summary>
    /// Context-based repository with synchronous and asynchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    internal class ContextRepository<TEntity> : IRepository<TEntity> where TEntity : class, IGuidResolvable, IValidatable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context">The context used by the repository</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="context"/> is null</exception>
        /// <exception cref="TypeLoadException">Occurs when TEntity is not a set type registered in the context</exception>
        public ContextRepository(DbContext context)
        {
            Context = context;
            if (Context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            Set = context.Set<TEntity>();
            if (Set is null)
            {
                throw new TypeLoadException($"The set of type {typeof(TEntity)} was not found in the context.");
            }
            RequiredProperties = new ReadOnlyCollection<PropertyInfo>(typeof(TEntity).GetProperties().Where((property) => Attribute.IsDefined(property, typeof(RequiredAttribute))).ToList());
            UniqueIndexesQueryBuilder = new(context.Model.GetEntityTypes());
        }

        /// <summary>
        /// Gets or sets the context used by the repository.
        /// </summary>
        protected virtual DbContext Context { get; init; }

        /// <summary>
        /// Gets or sets the required properties of the type of entity managed by the repository.
        /// </summary>
        protected virtual IReadOnlyCollection<PropertyInfo> RequiredProperties { get; init; }

        /// <summary>
        /// Gets or sets the context set used by the repository (operations are performed on it).
        /// </summary>
        protected virtual DbSet<TEntity> Set { get; init; }

        /// <summary>
        /// Gets or sets the query builder to check the integrity of unique indexes on entities.
        /// </summary>
        protected virtual UniqueIndexesQueryBuilder<TEntity> UniqueIndexesQueryBuilder { get; init; }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Add(TEntity)"/>
        public void Add(TEntity entity)
        {
            Set.Add(entity);
            Context.SaveChanges();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.AddAsync(TEntity)"/>
        public async Task AddAsync(TEntity entity)
        {
            Set.Add(entity);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.AddRange(IEnumerable{TEntity})"/>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
            Context.SaveChanges();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.AddRangeAsync(IEnumerable{TEntity})"/>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanDelete(TEntity)"/>
        public SerializationResult CanDelete(TEntity entity)
        {
            SerializationResult entityResult = entity.CanBeDeleted();
            if (!entityResult.IsOk)
            {
                return entityResult;
            }
            if (!IsInDatabase(entity))
            {
                return new(entity, new NotFoundInDatabase());
            }
            return new(entity, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanDeleteAsync(TEntity)"/>
        public async Task<SerializationResult> CanDeleteAsync(TEntity entity)
        {
            SerializationResult entityResult = entity.CanBeDeleted();
            if (!entityResult.IsOk)
            {
                return entityResult;
            }
            if (!await IsInDatabaseAsync(entity))
            {
                return new(entity, new NotFoundInDatabase());
            }
            return new(entity, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanDeleteRange(IEnumerable{TEntity})"/>
        public SerializationResult CanDeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                SerializationResult entityResult = entity.CanBeDeleted();
                if (!entityResult.IsOk)
                {
                    return entityResult;
                }
            }
            if (IsAnyNotInDatabase(entities) is TEntity notFoundEntity)
            {
                return new(notFoundEntity, new NotFoundInDatabase());
            }
            return new(null, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanDeleteRangeAsync(IEnumerable{TEntity})"/>
        public async Task<SerializationResult> CanDeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                SerializationResult entityResult = entity.CanBeDeleted();
                if (!entityResult.IsOk)
                {
                    return entityResult;
                }
            }
            if (await IsAnyNotInDatabaseAsync(entities) is TEntity notFoundEntity)
            {
                return new(notFoundEntity, new NotFoundInDatabase());
            }
            return new(null, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanSave(TEntity)"/>
        public SerializationResult CanSave(TEntity entity)
        {
            SerializationResult entityResult = entity.CanBeSavedOrUpdated();
            if (!entityResult.IsOk)
            {
                return entityResult;
            }
            if (IsInDatabase(entity))
            {
                return new(entity, new AlreadyExistsInDatabase());
            }
            if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
            {
                return new(entity, requiredError);
            }
            if (AreUniqueIndexesRespected(entity, null) is SerializationError indexError)
            {
                return new(entity, indexError);
            }
            return new(entity, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanSaveAsync(TEntity)"/>
        public async Task<SerializationResult> CanSaveAsync(TEntity entity)
        {
            SerializationResult entityResult = entity.CanBeSavedOrUpdated();
            if (!entityResult.IsOk)
            {
                return entityResult;
            }
            if (await IsInDatabaseAsync(entity))
            {
                return new(entity, new AlreadyExistsInDatabase());
            }
            if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
            {
                return new(entity, requiredError);
            }
            if (await AreUniqueIndexesRespectedAsync(entity, null) is SerializationError indexError)
            {
                return new(entity, indexError);
            }
            return new(entity, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanSaveRange(IEnumerable{TEntity})"/>
        public SerializationResult CanSaveRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                SerializationResult entityResult = entity.CanBeSavedOrUpdated();
                if (!entityResult.IsOk)
                {
                    return entityResult;
                }
            }
            if (IsAnyInDatabase(entities) is TEntity existingEntity)
            {
                return new(existingEntity, new AlreadyExistsInDatabase());
            }
            foreach (TEntity entity in entities)
            {
                if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
                {
                    return new(entity, requiredError);
                }
            }
            foreach (TEntity entity in entities)
            {
                if (AreUniqueIndexesRespected(entity, entities) is SerializationError indexError)
                {
                    return new(entity, indexError);
                }
            }
            return new(null, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanSaveRangeAsync(IEnumerable{TEntity})"/>
        public async Task<SerializationResult> CanSaveRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                SerializationResult entityResult = entity.CanBeSavedOrUpdated();
                if (!entityResult.IsOk)
                {
                    return entityResult;
                }
            }
            if (await IsAnyInDatabaseAsync(entities) is TEntity existingEntity)
            {
                return new(existingEntity, new AlreadyExistsInDatabase());
            }
            foreach (TEntity entity in entities)
            {
                if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
                {
                    return new(entity, requiredError);
                }
            }
            foreach (TEntity entity in entities)
            {
                if (await AreUniqueIndexesRespectedAsync(entity, entities) is SerializationError indexError)
                {
                    return new(entity, indexError);
                }
            }
            return new(null, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanUpdate(TEntity)"/>
        public SerializationResult CanUpdate(TEntity entity)
        {
            SerializationResult entityResult = entity.CanBeSavedOrUpdated();
            if (!entityResult.IsOk)
            {
                return entityResult;
            }
            if (!IsInDatabase(entity))
            {
                return new(entity, new NotFoundInDatabase());
            }
            if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
            {
                return new(entity, requiredError);
            }
            if (AreUniqueIndexesRespected(entity, null) is SerializationError indexError)
            {
                return new(entity, indexError);
            }
            return new(entity, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanUpdateAsync(TEntity)"/>
        public async Task<SerializationResult> CanUpdateAsync(TEntity entity)
        {
            SerializationResult entityResult = entity.CanBeSavedOrUpdated();
            if (!entityResult.IsOk)
            {
                return entityResult;
            }
            if (!await IsInDatabaseAsync(entity))
            {
                return new(entity, new NotFoundInDatabase());
            }
            if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
            {
                return new(entity, requiredError);
            }
            if (await AreUniqueIndexesRespectedAsync(entity, null) is SerializationError indexError)
            {
                return new(entity, indexError);
            }
            return new(entity, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanUpdateRange(IEnumerable{TEntity})"/>
        public SerializationResult CanUpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                SerializationResult entityResult = entity.CanBeSavedOrUpdated();
                if (!entityResult.IsOk)
                {
                    return entityResult;
                }
            }
            if (IsAnyNotInDatabase(entities) is TEntity notFoundEntity)
            {
                return new(notFoundEntity, new NotFoundInDatabase());
            }
            foreach (TEntity entity in entities)
            {
                if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
                {
                    return new(entity, requiredError);
                }
            }
            foreach (TEntity entity in entities)
            {
                if (AreUniqueIndexesRespected(entity, entities) is SerializationError indexError)
                {
                    return new(entity, indexError);
                }
            }
            return new(null, null);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanUpdateRangeAsync(IEnumerable{TEntity})"/>
        public async Task<SerializationResult> CanUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                SerializationResult entityResult = entity.CanBeSavedOrUpdated();
                if (!entityResult.IsOk)
                {
                    return entityResult;
                }
            }
            if (await IsAnyNotInDatabaseAsync(entities) is TEntity notFoundEntity)
            {
                return new(notFoundEntity, new NotFoundInDatabase());
            }
            foreach (TEntity entity in entities)
            {
                if (AreRequiredPropertiesFilled(entity) is SerializationError requiredError)
                {
                    return new(entity, requiredError);
                }
            }
            foreach (TEntity entity in entities)
            {
                if (await AreUniqueIndexesRespectedAsync(entity, entities) is SerializationError indexError)
                {
                    return new(entity, indexError);
                }
            }
            return new(null, null);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Delete(TEntity)"/>
        public void Delete(TEntity entity)
        {
            Set.Remove(entity);
            Context.SaveChanges();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.DeleteAsync(TEntity)"/>
        public async Task DeleteAsync(TEntity entity)
        {
            Set.Remove(entity);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.DeleteRange(IEnumerable{TEntity})"/>
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
            Context.SaveChanges();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.DeleteRangeAsync(IEnumerable{TEntity})"/>
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Find(Expression{Func{TEntity, bool}})"/>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Where(predicate).ToList();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindAll"/>
        public IEnumerable<TEntity> FindAll()
        {
            return Set.ToList();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindAllAsync"/>
        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await Set.ToListAsync();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindAsync(Expression{Func{TEntity, bool}})"/>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Set.Where(predicate).ToListAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindOne(Guid)"/>
        public TEntity FindOne(Guid id)
        {
            return Set.Find(id);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindOne(Expression{Func{TEntity, bool}})"/>
        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Where(predicate).FirstOrDefault();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindOneAsync(Guid)"/>
        public async Task<TEntity> FindOneAsync(Guid id)
        {
            return await Set.FindAsync(id);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindOneAsync(Expression{Func{TEntity, bool}})"/>
        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Set.Where(predicate).FirstOrDefaultAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsAnyInDatabase(IEnumerable{TEntity})"/>
        public TEntity IsAnyInDatabase(IEnumerable<TEntity> entities)
        {
            return entities.Where(entity => entity.Id != Guid.Empty).AsQueryable().Union(Set).FirstOrDefault();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsAnyInDatabaseAsync(IEnumerable{TEntity})"/>
        public async Task<TEntity> IsAnyInDatabaseAsync(IEnumerable<TEntity> entities)
        {
            return await entities.Where(entity => entity.Id != Guid.Empty).AsQueryable().Union(Set).FirstOrDefaultAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsAnyNotInDatabase(IEnumerable{TEntity})"/>
        public TEntity IsAnyNotInDatabase(IEnumerable<TEntity> entities)
        {
            return entities.AsQueryable().Except(Set).FirstOrDefault();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsAnyNotInDatabaseAsync(IEnumerable{TEntity})"/>
        public async Task<TEntity> IsAnyNotInDatabaseAsync(IEnumerable<TEntity> entities)
        {
            return await entities.AsQueryable().Except(Set).FirstOrDefaultAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsInDatabase(TEntity)"/>
        public bool IsInDatabase(TEntity entity)
        {
            return entity.Id != Guid.Empty && Set.Any((other) => entity.Id == other.Id);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsInDatabaseAsync(TEntity)"/>
        public async Task<bool> IsInDatabaseAsync(TEntity entity)
        {
            return entity.Id != Guid.Empty && await Set.AnyAsync((other) => entity.Id == other.Id);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Rollback(TEntity)"/>
        public void Rollback(TEntity entity)
        {
            Context.Entry(entity).Reload();
            Context.SaveChanges();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.RollbackAsync(TEntity)"/>
        public async Task RollbackAsync(TEntity entity)
        {
            await Context.Entry(entity).ReloadAsync().ContinueWith(delegate { Context.SaveChangesAsync(); });
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.RollbackRange(IEnumerable{TEntity})"/>
        public void RollbackRange(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                Context.Entry(entity).Reload();
            }
            Context.SaveChanges();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.RollbackRangeAsync(IEnumerable{TEntity})"/>
        public async Task RollbackRangeAsync(IEnumerable<TEntity> entities)
        {
            List<Task> tasks = new();
            foreach (TEntity entity in entities)
            {
                tasks.Add(Context.Entry(entity).ReloadAsync());
            }
            await Task.WhenAll(tasks).ContinueWith(delegate { Context.SaveChangesAsync(); });
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.SaveUpdate(TEntity)"/>
        public void SaveUpdate(TEntity entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.SaveUpdateAsync(TEntity)"/>
        public async Task SaveUpdateAsync(TEntity entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.SaveUpdateRange(IEnumerable{TEntity})"/>
        public void SaveUpdateRange(IEnumerable<TEntity> entities)
        {
            Context.UpdateRange(entities);
            Context.SaveChanges();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.SaveUpdateRangeAsync(IEnumerable{TEntity})"/>
        public async Task SaveUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            Context.UpdateRange(entities);
            await Context.SaveChangesAsync();
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
            IQueryable<TEntity> toCompare = Set;
            if (entitySet is not null)
            {
                toCompare = toCompare.Concat(entitySet);
            }
            if (toCompare.Where(UniqueIndexesQueryBuilder.BuildUniqueIndexesExpression(entity)).Where(item => entity.Id != item.Id).FirstOrDefault() is TEntity conflictItem)
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
        protected async Task<SerializationError> AreUniqueIndexesRespectedAsync(TEntity entity, IEnumerable<TEntity> entitySet)
        {
            IQueryable<TEntity> toCompare = Set;
            if (entitySet is not null)
            {
                toCompare = toCompare.Concat(entitySet);
            }
            if (await toCompare.Where(UniqueIndexesQueryBuilder.BuildUniqueIndexesExpression(entity)).Where(item => entity.Id != item.Id).FirstOrDefaultAsync() is TEntity conflictItem)
            {
                return new UniqueIndexViolated<Guid>(conflictItem.Id);
            }
            return null;
        }
    }
}