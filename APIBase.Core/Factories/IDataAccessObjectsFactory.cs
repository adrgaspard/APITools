using APIBase.Core.DAO.Models;
using APIBase.Core.DAO.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace APIBase.Core.Factories
{
    /// <summary>
    /// Represents all classes that instanciates some DAO objects.
    /// </summary>
    public interface IDataAccessObjectsFactory
    {
        /// <summary>
        /// Instanciate a new observable repository.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity managed by the observable repository</typeparam>
        /// <param name="context">Database context</param>
        /// <param name="baseRepository">The repository to use for operations (can be null)</param>
        /// <returns>The new observable repository</returns>
        IObservableRepository<TEntity> BuildObservableRepository<TEntity>(DbContext context, IRepository<TEntity> baseRepository = null) where TEntity : class, IGuidResolvable, IValidatable;

        /// <summary>
        /// Instanciate a new repository validator.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="repositories">List of all repositories</param>
        /// <returns>The new repository validator</returns>
        IRepositoryValidator BuildRepositoryValidator(DbContext context, IDictionary<Type, object> repositories);

        /// <summary>
        /// Instanciate a new repository.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity managed by the repository</typeparam>
        /// <param name="context">Database context</param>
        /// <returns>The new repository</returns>
        IRepository<TEntity> BuildSimpleRepository<TEntity>(DbContext context) where TEntity : class, IGuidResolvable, IValidatable;
    }
}