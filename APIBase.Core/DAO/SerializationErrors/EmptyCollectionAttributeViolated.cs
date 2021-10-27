namespace APIBase.Core.DAO.SerializationErrors
{
    /// <summary>
    /// Represents the serialization error when a collection attribute is empty when it shouldn't be.
    /// </summary>
    public class EmptyCollectionAttributeViolated : SinglePropertySerializationError
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="property">The property affected by the error at string format</param>
        public EmptyCollectionAttributeViolated(string property) : base(property)
        {
        }
    }
}