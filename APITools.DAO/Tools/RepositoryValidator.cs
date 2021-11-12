using APITools.Core.Base.DAO.Models;
using APITools.Core.Server.DAO.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace APITools.DAO.Tools
{
    /// <summary>
    /// A context-based read-only implementation to validate repositories with the database model.
    /// </summary>
    internal class RepositoryValidator : IRepositoryValidator
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context">The context to use</param>
        /// <param name="repositories">The repositories to be made known to the validator</param>
        public RepositoryValidator(DbContext context, IDictionary<Type, object> repositories)
        {
            Context = context;
            Repositories = GetCorrectRepositories(repositories);
            EntityTypesWithoutRepository = GetEntityTypesWithoutRepository();
        }

        /// <inheritdoc cref="IRepositoryValidator.EntityTypesWithoutRepository"/>
        public IReadOnlyCollection<Type> EntityTypesWithoutRepository { get; protected set; }

        /// <inheritdoc cref="IRepositoryValidator.Repositories"/>
        public IReadOnlyDictionary<Type, object> Repositories { get; protected set; }

        /// <summary>
        /// Gets or sets the context used by the repository validator.
        /// </summary>
        protected DbContext Context { get; set; }

        /// <summary>
        /// Checks which repositories are valid (they must directly or indirectly implement IAsyncRepository or ISyncRepository).
        /// </summary>
        /// <param name="repositories">The repositories to be checked</param>
        /// <returns>A dictionary which contains validated repositories only (the other are ignored)</returns>
        /// <see cref="ISyncRepository{TEntity}"/>
        /// <see cref="IAsyncRepository{TEntity}"/>
        protected static IReadOnlyDictionary<Type, object> GetCorrectRepositories(IDictionary<Type, object> repositories)
        {
            Dictionary<Type, object> validatedRepositories = new();
            if (repositories is not null)
            {
                foreach (KeyValuePair<Type, object> entry in repositories.Where(pair => pair.Key is not null && pair.Key.IsGenericType))
                {
                    Type entryKeyGenericType = entry.Key.GetGenericTypeDefinition();
                    if ((entryKeyGenericType.IsSubclassOf(typeof(ISyncRepository<>)) || entryKeyGenericType.IsSubclassOf(typeof(IAsyncRepository<>))) && (entry.Value is null || entry.Key.GetGenericArguments().FirstOrDefault().Equals(entry.Value.GetType().GetGenericArguments().FirstOrDefault())))
                    {
                        validatedRepositories.Add(entry.Key, entry.Value);
                    }
                }
            }
            return new ReadOnlyDictionary<Type, object>(validatedRepositories);
        }

        /// <summary>
        /// Go through the model described by the context and find the entity types that do not have a valid repository.
        /// </summary>
        /// <returns>A collection of all entity types found in the model without a valid repository</returns>
        protected IReadOnlyCollection<Type> GetEntityTypesWithoutRepository()
        {
            IEnumerable<Type> types = Context.Model.GetEntityTypes().Select(entityType => entityType.ClrType);
            List<Type> result = new(types.Where(type => !type.IsSubclassOf(typeof(IGuidResolvable)) || !type.IsSubclassOf(typeof(IValidatable))));
            foreach (Type entityType in types.Where(type => !result.Contains(type)))
            {
                Type repositoryType = typeof(IRepository<>).MakeGenericType(new[] { entityType });
                Type syncRepositoryType = typeof(ISyncRepository<>).MakeGenericType(new[] { entityType });
                Type asyncRepositoryType = typeof(IAsyncRepository<>).MakeGenericType(new[] { entityType });
                if (!Repositories.ContainsKey(repositoryType) && !Repositories.ContainsKey(syncRepositoryType) && !Repositories.ContainsKey(asyncRepositoryType))
                {
                    result.Add(entityType);
                }
            }
            return new ReadOnlyCollection<Type>(result);
        }
    }
}