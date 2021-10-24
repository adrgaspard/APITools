﻿using APIBase.Core.ComponentModel;
using APIBase.Core.DAO.Models;
using System;

namespace APIBase.Core.DAO.Repositories
{
    /// <summary>
    /// Represents all repository types that can notify other objects with synchronous and asynchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    public interface IObservableRepository<TEntity> : IRepository<TEntity>, IObservableSyncRepository<TEntity>, IObservableAsyncRepository<TEntity>, INotifyItemOperationPerformed<Guid> where TEntity : class, IGuidResolvable, IValidatable
    {
    }
}