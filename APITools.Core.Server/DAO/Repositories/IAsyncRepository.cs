using APITools.Core.Base.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APITools.Core.Server.DAO.Repositories
{
    /// <summary>
    /// Represents all repository types with asynchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    public interface IAsyncRepository<TEntity> where TEntity : class, IGuidResolvable, IValidatable
    {
        /// <summary>
        /// Add asynchronously an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to be added to the repository</param>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Add asynchronously many entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to be added to the repository</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks asynchronously if an entity can be deleted with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess or SetSerializationResultOnError.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>A value that indicates whether the object can be deleted with the repository or not</returns>
        Task<SerializationResult> CanDeleteAsync(TEntity entity);

        /// <summary>
        /// Checks asynchronously if some entities can be deleted with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess on entities that did not cause an error and/or SetSerializationResultOnError on the entity that caused the error.
        /// </summary>
        /// <param name="entities">The entities to check</param>
        /// <returns>A value that indicates whether the objects can be deleted with the repository or not</returns>
        Task<SerializationResult> CanDeleteRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks asynchronously if an entity can be saved with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess or SetSerializationResultOnError.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>A value that indicates whether the object can be saved with the repository or not</returns>
        Task<SerializationResult> CanSaveAsync(TEntity entity);

        /// <summary>
        /// Checks asynchronously if some entities can be saved with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess on entities that did not cause an error and/or SetSerializationResultOnError on the entity that caused the error.
        /// </summary>
        /// <param name="entities">The entities to check</param>
        /// <returns>A value that indicates whether the objects can be saved with the repository or not</returns>
        Task<SerializationResult> CanSaveRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks asynchronously if an entity can be updated with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess or SetSerializationResultOnError.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>A value that indicates whether the object can be updated with the repository or not</returns>
        Task<SerializationResult> CanUpdateAsync(TEntity entity);

        /// <summary>
        /// Checks asynchronously if some entities can be updated with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess on entities that did not cause an error and/or SetSerializationResultOnError on the entity that caused the error.
        /// </summary>
        /// <param name="entities">The entities to check</param>
        /// <returns>A value that indicates whether the objects can be updated with the repository or not</returns>
        Task<SerializationResult> CanUpdateRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete asynchronously an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted from the repository</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete asynchronously many entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to be deleted from the repository</param>
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Find asynchronously all the entities of the repository.
        /// </summary>
        /// <returns>A collection that contains all the entities of the repository</returns>
        Task<IEnumerable<TEntity>> FindAllAsync();

        /// <summary>
        /// Find asynchronously all entities that matches the predicate.
        /// </summary>
        /// <param name="predicate">The predicate to be matched</param>
        /// <returns>A collection that contains all entities found that matches the predicate</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find asynchronously an entity with a specific identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to be retrieved</param>
        /// <returns>The entity found with the specified identifier (null if no entity was found)</returns>
        Task<TEntity> FindOneAsync(Guid id);

        /// <summary>
        /// Find asynchronously an entity that matches the predicate.
        /// </summary>
        /// <param name="predicate">The predicate to be matched</param>
        /// <returns>The first entity found that matches the predicate (null if no entity was found)</returns>
        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Checks asynchronously if at least one entity exists in the database.
        /// </summary>
        /// <param name="entities">The entities to be checked</param>
        /// <returns>The entity which is in the database (or null if there is none)</returns>
        Task<TEntity> IsAnyInDatabaseAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks asynchronously if at least one entity do not exists in the database.
        /// </summary>
        /// <param name="entities">The entities to be checked</param>
        /// <returns>The entity which is not in the database (or null if there is none)</returns>
        Task<TEntity> IsAnyNotInDatabaseAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks asynchronously if an entity exists in the database.
        /// It also calls on the entity SetSerializationResultOnSuccess or SetSerializationResultOnError.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>A value that
        /// whether the object can be saved with the repository or not</returns>
        Task<bool> IsInDatabaseAsync(TEntity entity);

        /// <summary>
        /// Restore asynchronously the last backup of an entity.
        /// </summary>
        /// <param name="entity">The entity to be restored from the repository</param>
        Task RollbackAsync(TEntity entity);

        /// <summary>
        /// Restore asynchronously the last backup of many entities.
        /// </summary>
        /// <param name="entities">The entities to be restored from the repository</param>
        Task RollbackRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Save asynchronously the updates of an entity on the repository.
        /// </summary>
        /// <param name="entity">The entity to be updated on the repository</param>
        Task SaveUpdateAsync(TEntity entity);

        /// <summary>
        /// Save asynchronously the updates of many entities on the repository.
        /// </summary>
        /// <param name="entities">The entities to be updated on the repository</param>
        Task SaveUpdateRangeAsync(IEnumerable<TEntity> entities);
    }
}