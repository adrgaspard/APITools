using APITools.Core.ComponentModel;
using APITools.Core.DAO.Models;
using System;

namespace APITools.Core.Server.DAO.Repositories
{
    /// <summary>
    /// Represents all repository types that can notify other objects with asynchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    public interface IObservableAsyncRepository<TEntity> : IAsyncRepository<TEntity>, INotifyItemOperationPerformed<Guid> where TEntity : class, IGuidResolvable, IValidatable
    {
    }
}