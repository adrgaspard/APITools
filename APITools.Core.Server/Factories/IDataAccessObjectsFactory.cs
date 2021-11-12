using APITools.Core.Base.DAO.Models;
using APITools.Core.Server.DAO.Repositories;
using System;
using System.Collections.Generic;

namespace APITools.Core.Server.Factories
{
    /// <summary>
    /// Represents all classes that instanciates some DAO objects.
    /// </summary>
    public interface IDataAccessObjectsFactory
    {
        /// <summary>
        /// Instanciate a new observable repository from an existing repository.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity managed by the observable repository</typeparam>
        /// <param name="baseRepository">The repository to use for operations (can be null)</param>
        /// <returns>The new observable repository</returns>
        IObservableRepository<TEntity> BuildObservableRepository<TEntity>(IRepository<TEntity> baseRepository) where TEntity : class, IGuidResolvable, IValidatable;

        /// <summary>
        /// Instanciate a new observable repository.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity managed by the observable repository</typeparam>
        /// <param name="sourceType">The source type of the repository</param>
        /// <returns>The new observable repository</returns>
        IObservableRepository<TEntity> BuildObservableRepository<TEntity>(RepositorySourceType sourceType) where TEntity : class, IGuidResolvable, IValidatable;

        /// <summary>
        /// Instanciate a new repository validator.
        /// </summary>
        /// <param name="repositories">List of all repositories</param>
        /// <returns>The new repository validator</returns>
        IRepositoryValidator BuildRepositoryValidator(IDictionary<Type, object> repositories);

        /// <summary>
        /// Instanciate a new repository.
        /// </summary>
        /// <typeparam name="TEntity">Type of the entity managed by the repository</typeparam>
        /// <param name="sourceType">The source type of the repository</param>
        /// <returns>The new repository</returns>
        IRepository<TEntity> BuildSimpleRepository<TEntity>(RepositorySourceType sourceType) where TEntity : class, IGuidResolvable, IValidatable;
    }
}