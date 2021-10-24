using APIBase.Core.DAO.Models;
using APIBase.Core.DAO.Repositories;
using APIBase.Core.Factories;
using APIBase.DAO.Repositories;
using APIBase.DAO.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace APIBase.DAO.Factories
{
    /// <summary>
    /// Instantiator of DAO related objects, with default implementations.
    /// </summary>
    public class DataAccessObjectsFactory : IDataAccessObjectsFactory
    {
        /// <inheritdoc cref="IDataAccessObjectsFactory.BuildObservableRepository{TEntity}(DbContext, IRepository{TEntity})"/>
        public IObservableRepository<TEntity> BuildObservableRepository<TEntity>(DbContext context, IRepository<TEntity> baseRepository = null) where TEntity : class, IGuidResolvable, IValidatable
        {
            if (baseRepository is null)
            {
                baseRepository = BuildSimpleRepository<TEntity>(context);
            }
            return new ObservableRepository<TEntity>(baseRepository);
        }

        /// <inheritdoc cref="IDataAccessObjectsFactory.BuildRepositoryValidator(DbContext, IDictionary{Type, object})"/>
        public IRepositoryValidator BuildRepositoryValidator(DbContext context, IDictionary<Type, object> repositories)
        {
            return new RepositoryValidator(context, repositories);
        }

        /// <inheritdoc cref="IDataAccessObjectsFactory.BuildSimpleRepository{TEntity}(DbContext)"/>
        public IRepository<TEntity> BuildSimpleRepository<TEntity>(DbContext context) where TEntity : class, IGuidResolvable, IValidatable
        {
            return new Repository<TEntity>(context);
        }
    }
}