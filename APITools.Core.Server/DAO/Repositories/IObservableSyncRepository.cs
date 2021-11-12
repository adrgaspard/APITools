﻿using APITools.Core.Base.ComponentModel;
using APITools.Core.Base.DAO.Models;
using System;

namespace APITools.Core.Server.DAO.Repositories
{
    /// <summary>
    /// Represents all repository types that can notify other objects with synchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    public interface IObservableSyncRepository<TEntity> : ISyncRepository<TEntity>, INotifyItemOperationPerformed<Guid> where TEntity : class, IGuidResolvable, IValidatable
    {
    }
}