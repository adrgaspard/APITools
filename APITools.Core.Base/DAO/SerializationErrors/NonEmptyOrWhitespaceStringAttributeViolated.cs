namespace APITools.Core.DAO.SerializationErrors
{
    /// <summary>
    /// Represents the serialization error when a string attribute is empty or consist only of white-space characters when it shouldn't be.
    /// </summary>
    public class NonEmptyOrWhitespaceStringAttributeViolated : SinglePropertySerializationError
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="property">The property affected by the error at string format</param>
        public NonEmptyOrWhitespaceStringAttributeViolated(string property) : base(property)
        {
        }
    }
}