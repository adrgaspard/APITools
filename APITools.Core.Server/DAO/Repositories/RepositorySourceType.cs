namespace APITools.Core.Server.DAO.Repositories
{
    /// <summary>
    /// Represents all the options for a repository.
    /// </summary>
    public enum RepositorySourceType
    {
        /// <summary>
        /// The repository will use a DbContext as a source (use this mode by default).
        /// </summary>
        DbContext = 0,

        /// <summary>
        /// The repository will store all items in memory (which means that it will be empty when the application is launched).
        /// </summary>
        Memory = 1,

        /// <summary>
        /// The repository will block any operation with items (useful when you want to explicitly forbid any operation with an entity type).
        /// </summary>
        Ban = 2
    }
}