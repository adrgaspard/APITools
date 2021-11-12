using APITools.Core.Base.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace APITools.Core.Server.DAO.Repositories
{
    /// <summary>
    /// Represents all repository types with synchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    public interface ISyncRepository<TEntity> where TEntity : class, IGuidResolvable, IValidatable
    {
        /// <summary>
        /// Add synchronously an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to be added to the repository</param>
        void Add(TEntity entity);

        /// <summary>
        /// Add synchronously many entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to be added to the repository</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks synchronously if an entity can be deleted with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess or SetSerializationResultOnError.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>A value that indicates whether the object can be deleted with the repository or not</returns>
        SerializationResult CanDelete(TEntity entity);

        /// <summary>
        /// Checks synchronously if some entities can be deleted with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess on entities that did not cause an error and/or SetSerializationResultOnError on the entity that caused the error.
        /// </summary>
        /// <param name="entities">The entities to check</param>
        /// <returns>A value that indicates whether the objects can be deleted with the repository or not</returns>
        SerializationResult CanDeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks synchronously if an entity can be saved with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess or SetSerializationResultOnError.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>A value that indicates whether the object can be saved with the repository or not</returns>
        SerializationResult CanSave(TEntity entity);

        /// <summary>
        /// Checks synchronously if some entities can be saved with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess on entities that did not cause an error and/or SetSerializationResultOnError on the entity that caused the error.
        /// </summary>
        /// <param name="entities">The entities to check</param>
        /// <returns>A value that indicates whether the objects can be saved with the repository or not</returns>
        SerializationResult CanSaveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks synchronously if an entity can be updated with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess or SetSerializationResultOnError.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>A value that indicates whether the object can be updated with the repository or not</returns>
        SerializationResult CanUpdate(TEntity entity);

        /// <summary>
        /// Checks synchronously if some entities can be updated with the repository.
        /// It also calls on the entity SetSerializationResultOnSuccess on entities that did not cause an error and/or SetSerializationResultOnError on the entity that caused the error.
        /// </summary>
        /// <param name="entities">The entities to check</param>
        /// <returns>A value that indicates whether the objects can be deleted with the repository or not</returns>
        SerializationResult CanUpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete synchronously an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to be deleted from the repository</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete synchronously many entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to be deleted from the repository</param>
        void DeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Find synchronously all entities that matches the predicate.
        /// </summary>
        /// <param name="predicate">The predicate to be matched</param>
        /// <returns>A collection that contains all entities found that matches the predicate</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find synchronously all the entities of the repository.
        /// </summary>
        /// <returns>A collection that contains all the entities of the repository</returns>
        IEnumerable<TEntity> FindAll();

        /// <summary>
        /// Find synchronously an entity with a specific identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to be retrieved</param>
        /// <returns>The entity found with the specified identifier (null if no entity was found)</returns>
        TEntity FindOne(Guid id);

        /// <summary>
        /// Find synchronously an entity that matches the predicate.
        /// </summary>
        /// <param name="predicate">The predicate to be matched</param>
        /// <returns>The first entity found that matches the predicate (null if no entity was found)</returns>
        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Checks synchronously if at least one entity exists in the database.
        /// </summary>
        /// <param name="entities">The entities to be checked</param>
        /// <returns>The entity which is in the database (or null if there is none)</returns>
        TEntity IsAnyInDatabase(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks synchronously if at least one entity do not exists in the database.
        /// </summary>
        /// <param name="entities">The entities to be checked</param>
        /// <returns>The entity which is not in the database (or null if there is none)</returns>
        TEntity IsAnyNotInDatabase(IEnumerable<TEntity> entities);

        /// <summary>
        /// Checks synchronously if an entity exists in the database.
        /// It also calls on the entity SetSerializationResultOnSuccess or SetSerializationResultOnError.
        /// </summary>
        /// <param name="entity">The entity to check</param>
        /// <returns>A value that indicates whether the object can be saved with the repository or not</returns>
        bool IsInDatabase(TEntity entity);

        /// <summary>
        /// Restore synchronously the last backup of an entity.
        /// </summary>
        /// <param name="entity">The entity to be restored from the repository</param>
        void Rollback(TEntity entity);

        /// <summary>
        /// Restore synchronously the last backup of many entities.
        /// </summary>
        /// <param name="entities">The entities to be restored from the repository</param>
        void RollbackRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Save synchronously the updates of an entity on the repository.
        /// </summary>
        /// <param name="entity">The entity to be updated on the repository</param>
        void SaveUpdate(TEntity entity);

        /// <summary>
        /// Save synchronously the updates of many entities on the repository.
        /// </summary>
        /// <param name="entities">The entities to be updated on the repository</param>
        void SaveUpdateRange(IEnumerable<TEntity> entities);
    }
}