using APIBase.Core.ComponentModel;
using APIBase.Core.DAO.Models;
using APIBase.Core.DAO.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APIBase.DAO.Repositories
{
    /// <summary>
    /// Overlay-based repository that can notify other objects with synchronous and asynchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    internal class ObservableRepository<TEntity> : IObservableRepository<TEntity> where TEntity : class, IGuidResolvable, IValidatable
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="baseRepository">The base repository used by the repository</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="baseRepository"/> is null</exception>
        public ObservableRepository(IRepository<TEntity> baseRepository)
        {
            if (baseRepository is null)
            {
                throw new ArgumentNullException(nameof(baseRepository));
            }
            BaseRepository = baseRepository;
        }

        /// <inheritdoc cref="INotifyItemOperationPerformed{TItemId}.ItemOperationPerformed"/>
        public event EventHandler<SetItemOperationPerformedEventArgs<Guid>> ItemOperationPerformed;

        /// <summary>
        /// Gets or sets the base repository used by the repository.
        /// </summary>
        protected IRepository<TEntity> BaseRepository { get; init; }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Add(TEntity)"/>
        public void Add(TEntity entity)
        {
            BaseRepository.Add(entity);
            RaiseItemOperationPerformed(OperationType.Create, new List<Guid> { entity.Id });
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.AddAsync(TEntity)"/>
        public async Task AddAsync(TEntity entity)
        {
            await BaseRepository.AddAsync(entity).ContinueWith(delegate { RaiseItemOperationPerformed(OperationType.Create, new List<Guid> { entity.Id }); });
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.AddRange(IEnumerable{TEntity})"/>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            BaseRepository.AddRange(entities);
            RaiseItemOperationPerformed(OperationType.Create, entities.Select(entity => entity.Id));
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.AddRangeAsync(IEnumerable{TEntity})"/>
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await BaseRepository.AddRangeAsync(entities).ContinueWith(delegate { RaiseItemOperationPerformed(OperationType.Create, entities.Select(entity => entity.Id)); });
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanDelete(TEntity)"/>
        public SerializationResult CanDelete(TEntity entity)
        {
            return BaseRepository.CanDelete(entity);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanDeleteAsync(TEntity)"/>
        public async Task<SerializationResult> CanDeleteAsync(TEntity entity)
        {
            return await BaseRepository.CanDeleteAsync(entity);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanDeleteRange(IEnumerable{TEntity})"/>
        public SerializationResult CanDeleteRange(IEnumerable<TEntity> entities)
        {
            return BaseRepository.CanDeleteRange(entities);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanDeleteRangeAsync(IEnumerable{TEntity})"/>
        public async Task<SerializationResult> CanDeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            return await BaseRepository.CanDeleteRangeAsync(entities);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanSave(TEntity)"/>
        public SerializationResult CanSave(TEntity entity)
        {
            return BaseRepository.CanSave(entity);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanSaveAsync(TEntity)"/>
        public async Task<SerializationResult> CanSaveAsync(TEntity entity)
        {
            return await BaseRepository.CanSaveAsync(entity);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanSaveRange(IEnumerable{TEntity})"/>
        public SerializationResult CanSaveRange(IEnumerable<TEntity> entities)
        {
            return BaseRepository.CanSaveRange(entities);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanSaveRangeAsync(IEnumerable{TEntity})"/>
        public async Task<SerializationResult> CanSaveRangeAsync(IEnumerable<TEntity> entities)
        {
            return await BaseRepository.CanSaveRangeAsync(entities);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanUpdate(TEntity)"/>
        public SerializationResult CanUpdate(TEntity entity)
        {
            return BaseRepository.CanUpdate(entity);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanUpdateAsync(TEntity)"/>
        public async Task<SerializationResult> CanUpdateAsync(TEntity entity)
        {
            return await BaseRepository.CanUpdateAsync(entity);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.CanUpdateRange(IEnumerable{TEntity})"/>
        public SerializationResult CanUpdateRange(IEnumerable<TEntity> entities)
        {
            return BaseRepository.CanUpdateRange(entities);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.CanUpdateRangeAsync(IEnumerable{TEntity})"/>
        public async Task<SerializationResult> CanUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            return await BaseRepository.CanUpdateRangeAsync(entities);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Delete(TEntity)"/>
        public void Delete(TEntity entity)
        {
            BaseRepository.Delete(entity);
            RaiseItemOperationPerformed(OperationType.Delete, new List<Guid> { entity.Id });
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.DeleteAsync(TEntity)"/>
        public async Task DeleteAsync(TEntity entity)
        {
            await BaseRepository.DeleteAsync(entity).ContinueWith(delegate { RaiseItemOperationPerformed(OperationType.Delete, new List<Guid> { entity.Id }); });
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.DeleteRange(IEnumerable{TEntity})"/>
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            BaseRepository.DeleteRange(entities);
            RaiseItemOperationPerformed(OperationType.Delete, entities.Select(entity => entity.Id));
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.DeleteRangeAsync(IEnumerable{TEntity})"/>
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            await BaseRepository.DeleteRangeAsync(entities).ContinueWith(delegate { RaiseItemOperationPerformed(OperationType.Delete, entities.Select(entity => entity.Id)); });
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Find(Expression{Func{TEntity, bool}})"/>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return BaseRepository.Find(predicate);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindAll"/>
        public IEnumerable<TEntity> FindAll()
        {
            return BaseRepository.FindAll();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindAllAsync"/>
        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await BaseRepository.FindAllAsync();
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindAsync(Expression{Func{TEntity, bool}})"/>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await BaseRepository.FindAsync(predicate);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindOne(Guid)"/>
        public TEntity FindOne(Guid id)
        {
            return BaseRepository.FindOne(id);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.FindOne(Expression{Func{TEntity, bool}})"/>
        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return BaseRepository.FindOne(predicate);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindOneAsync(Guid)"/>
        public async Task<TEntity> FindOneAsync(Guid id)
        {
            return await BaseRepository.FindOneAsync(id);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.FindOneAsync(Expression{Func{TEntity, bool}})"/>
        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await BaseRepository.FindOneAsync(predicate);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsAnyInDatabase(IEnumerable{TEntity})"/>
        public TEntity IsAnyInDatabase(IEnumerable<TEntity> entities)
        {
            return BaseRepository.IsAnyInDatabase(entities);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsAnyInDatabaseAsync(IEnumerable{TEntity})"/>
        public async Task<TEntity> IsAnyInDatabaseAsync(IEnumerable<TEntity> entities)
        {
            return await BaseRepository.IsAnyInDatabaseAsync(entities);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsAnyNotInDatabase(IEnumerable{TEntity})"/>
        public TEntity IsAnyNotInDatabase(IEnumerable<TEntity> entities)
        {
            return BaseRepository.IsAnyNotInDatabase(entities);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsAnyNotInDatabaseAsync(IEnumerable{TEntity})"/>
        public async Task<TEntity> IsAnyNotInDatabaseAsync(IEnumerable<TEntity> entities)
        {
            return await BaseRepository.IsAnyNotInDatabaseAsync(entities);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.IsInDatabase(TEntity)"/>
        public bool IsInDatabase(TEntity entity)
        {
            return BaseRepository.IsInDatabase(entity);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.IsInDatabaseAsync(TEntity)"/>
        public async Task<bool> IsInDatabaseAsync(TEntity entity)
        {
            return await BaseRepository.IsInDatabaseAsync(entity);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.Rollback(TEntity)"/>
        public void Rollback(TEntity entity)
        {
            BaseRepository.Rollback(entity);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.RollbackAsync(TEntity)"/>
        public async Task RollbackAsync(TEntity entity)
        {
            await BaseRepository.RollbackAsync(entity);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.RollbackRange(IEnumerable{TEntity})"/>
        public void RollbackRange(IEnumerable<TEntity> entities)
        {
            BaseRepository.RollbackRange(entities);
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.RollbackRangeAsync(IEnumerable{TEntity})"/>
        public async Task RollbackRangeAsync(IEnumerable<TEntity> entities)
        {
            await BaseRepository.RollbackRangeAsync(entities);
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.SaveUpdate(TEntity)"/>
        public void SaveUpdate(TEntity entity)
        {
            BaseRepository.SaveUpdate(entity);
            RaiseItemOperationPerformed(OperationType.Update, new List<Guid> { entity.Id });
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.SaveUpdateAsync(TEntity)"/>
        public async Task SaveUpdateAsync(TEntity entity)
        {
            await BaseRepository.SaveUpdateAsync(entity).ContinueWith(delegate { RaiseItemOperationPerformed(OperationType.Update, new List<Guid> { entity.Id }); });
        }

        /// <inheritdoc cref="ISyncRepository{TEntity}.SaveUpdateRange(IEnumerable{TEntity})"/>
        public void SaveUpdateRange(IEnumerable<TEntity> entities)
        {
            BaseRepository.SaveUpdateRange(entities);
            RaiseItemOperationPerformed(OperationType.Update, entities.Select(entity => entity.Id));
        }

        /// <inheritdoc cref="IAsyncRepository{TEntity}.SaveUpdateRangeAsync(IEnumerable{TEntity})"/>
        public async Task SaveUpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            await BaseRepository.SaveUpdateRangeAsync(entities).ContinueWith(delegate { RaiseItemOperationPerformed(OperationType.Update, entities.Select(entity => entity.Id)); });
        }

        /// <summary>
        /// Trigger the event that signals that an operation has been performed.
        /// </summary>
        /// <param name="operationType">Type of the performed operation</param>
        /// <param name="impactedItemsIds">List of identifiers of the items impacted by the operation</param>
        protected void RaiseItemOperationPerformed(OperationType operationType, IEnumerable<Guid> impactedItemsIds)
        {
            ItemOperationPerformed?.Invoke(this, new SetItemOperationPerformedEventArgs<Guid>(operationType, impactedItemsIds));
        }
    }
}