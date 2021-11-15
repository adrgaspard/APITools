using APITools.Core.Base.DAO.Models;
using APITools.Core.Server.DAO.Repositories;
using APITools.Core.Server.Factories;
using APITools.DAO.Repositories;
using APITools.DAO.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace APITools.DAO.Factories
{
    /// <summary>
    /// Instantiator of DAO related objects, with default implementations.
    /// </summary>
    public class DataAccessObjectsFactory : IDataAccessObjectsFactory
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context">The database context used by the factory</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="context"/> is null</exception>
        public DataAccessObjectsFactory(DbContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            Context = context;
        }

        /// <summary>
        /// Gets or sets the database context used by the factory.
        /// </summary>
        protected DbContext Context { get; init; }

        /// <inheritdoc cref="IDataAccessObjectsFactory.BuildObservableRepository{TEntity}(IRepository{TEntity})"/>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="baseRepository"/> is null</exception>
        public IObservableRepository<TEntity> BuildObservableRepository<TEntity>(IRepository<TEntity> baseRepository) where TEntity : class, IGuidResolvable, IValidatable
        {
            if (baseRepository is null)
            {
                throw new ArgumentNullException(nameof(baseRepository));
            }
            return new ObservableRepository<TEntity>(baseRepository);
        }

        /// <inheritdoc cref="IDataAccessObjectsFactory.BuildObservableRepository{TEntity}(RepositorySourceType)"/>
        public IObservableRepository<TEntity> BuildObservableRepository<TEntity>(RepositorySourceType sourceType) where TEntity : class, IGuidResolvable, IValidatable
        {
            return BuildObservableRepository(BuildSimpleRepository<TEntity>(sourceType));
        }

        /// <inheritdoc cref="IDataAccessObjectsFactory.BuildRepositoryValidator(IDictionary{Type, object})"/>
        public IRepositoryValidator BuildRepositoryValidator(IDictionary<Type, object> repositories)
        {
            return new RepositoryValidator(Context, repositories);
        }

        /// <inheritdoc cref="IDataAccessObjectsFactory.BuildSimpleRepository{TEntity}(RepositorySourceType)"/>
        public IRepository<TEntity> BuildSimpleRepository<TEntity>(RepositorySourceType sourceType) where TEntity : class, IGuidResolvable, IValidatable
        {
            return sourceType switch
            {
                RepositorySourceType.DbContext => new ContextRepository<TEntity>(Context),
                RepositorySourceType.Memory => new MemoryRepository<TEntity>(Context.Model.GetEntityTypes()),
                RepositorySourceType.Ban => new ProhibitorRepository<TEntity>(),
                _ => throw new NotSupportedException($"The source type {sourceType} is not supported by the {nameof(DataAccessObjectsFactory)}"),
            };
        }
    }
}