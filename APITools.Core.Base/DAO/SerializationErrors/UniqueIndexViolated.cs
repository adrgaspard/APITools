namespace APITools.Core.DAO.SerializationErrors
{
    /// <summary>
    /// Represents the serialization error when an entity don't respects at least one unique index.
    /// </summary>
    /// <typeparam name="TIdentifier">Type of the identifier of the entity affected by the error</typeparam>
    public class UniqueIndexViolated<TIdentifier> : SerializationError where TIdentifier : struct
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="itemInConflictId">The identifier of the entity affected by the error</param>
        public UniqueIndexViolated(TIdentifier itemInConflictId)
        {
            ItemInConflictId = itemInConflictId;
        }

        /// <summary>
        /// Gets or sets the identifier of the entity affected by the error.
        /// </summary>
        public TIdentifier ItemInConflictId { get; init; }
    }
}