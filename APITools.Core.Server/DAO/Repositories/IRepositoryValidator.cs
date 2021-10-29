using System;
using System.Collections.Generic;

namespace APITools.Core.Server.DAO.Repositories
{
    /// <summary>
    /// Represents all classes that check the completeness a set of repositories.
    /// </summary>
    public interface IRepositoryValidator
    {
        /// <summary>
        /// Gets all entity types without repository (a null repository is different than no repository).
        /// </summary>
        IReadOnlyCollection<Type> EntityTypesWithoutRepository { get; }

        /// <summary>
        /// Gets a read-only dictionary that contains all repositories types in keys and contains all repositories instance in values (a repository instance can be null).
        /// </summary>
        IReadOnlyDictionary<Type, object> Repositories { get; }
    }
}