using APITools.Core.Base.DAO.Models;
using APITools.Core.Server.DAO.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace APITools.DAO.Repositories
{
    /// <summary>
    /// Prohibitor repository with synchronous and asynchronous operations. Each of them throws an exception on call.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    internal class ProhibitorRepository<TEntity> : IRepository<TEntity> where TEntity : class, IGuidResolvable, IValidatable
    {
        /// <inheritdoc cref="ISyncRepository{TEntity}.Add(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public void Add(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.AddAsync(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task AddAsync(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.AddRange(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.AddRangeAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanDelete(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public SerializationResult CanDelete(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanDeleteAsync(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<SerializationResult> CanDeleteAsync(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanDeleteRange(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public SerializationResult CanDeleteRange(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanDeleteRangeAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<SerializationResult> CanDeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanSave(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public SerializationResult CanSave(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanSaveAsync(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<SerializationResult> CanSaveAsync(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanSaveRange(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public SerializationResult CanSaveRange(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanSaveRangeAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<SerializationResult> CanSaveRangeAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanUpdate(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public SerializationResult CanUpdate(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanUpdateAsync(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<SerializationResult> CanUpdateAsync(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanUpdateRange(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public SerializationResult CanUpdateRange(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanUpdateRangeAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<SerializationResult> CanUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Delete(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public void Delete(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.DeleteAsync(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task DeleteAsync(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.DeleteRange(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.DeleteRangeAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Find(Expression{Func{TEntity, bool}})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindAll"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public IEnumerable<TEntity> FindAll()
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindAllAsync"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<IEnumerable<TEntity>> FindAllAsync()
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindAsync(Expression{Func{TEntity, bool}})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindOne(Guid)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public TEntity FindOne(Guid id)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindOne(Expression{Func{TEntity, bool}})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindOneAsync(Guid)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<TEntity> FindOneAsync(Guid id)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindOneAsync(Expression{Func{TEntity, bool}})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsAnyInDatabase(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public TEntity IsAnyInDatabase(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsAnyInDatabaseAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<TEntity> IsAnyInDatabaseAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsAnyNotInDatabase(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public TEntity IsAnyNotInDatabase(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsAnyNotInDatabaseAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<TEntity> IsAnyNotInDatabaseAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsInDatabase(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public bool IsInDatabase(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsInDatabaseAsync(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task<bool> IsInDatabaseAsync(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Rollback(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public void Rollback(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.RollbackAsync(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task RollbackAsync(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.RollbackRange(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public void RollbackRange(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.RollbackRangeAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task RollbackRangeAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.SaveUpdate(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public void SaveUpdate(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.SaveUpdateAsync(TEntity)"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task SaveUpdateAsync(TEntity entity)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.SaveUpdateRange(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public void SaveUpdateRange(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.SaveUpdateRangeAsync(IEnumerable{TEntity})"/>
        /// <exception cref="UnauthorizedAccessException">Occurs each time the method is called</exception>
        public Task SaveUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            throw ProhibitsCall();
        }

        /// <summary>
        /// Prohibits a call by instanciating an exception.
        /// </summary>
        /// <param name="callerName">Name of the caller method/property</param>*
        /// <returns>The instanciated exception</returns>
        protected UnauthorizedAccessException ProhibitsCall([CallerMemberName] string callerName = null)
        {
            return new UnauthorizedAccessException($"This repository does not allow to call the {callerName} method/property");
        }
    }
}
