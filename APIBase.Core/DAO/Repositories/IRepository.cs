using APIBase.Core.DAO.Models;

namespace APIBase.Core.DAO.Repositories
{
    /// <summary>
    /// Represents all repository types with synchronous and asynchronous operations for an identifiable and validatable object type.
    /// </summary>
    /// <typeparam name="TEntity">Type of the object to be stored with the repository</typeparam>
    public interface IRepository<TEntity> : IAsyncRepository<TEntity>, ISyncRepository<TEntity> where TEntity : class, IGuidResolvable, IValidatable
    {
    }
}