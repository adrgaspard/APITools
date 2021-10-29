namespace APITools.Core.DAO.SerializationErrors
{
    /// <summary>
    /// Represents the serialization error when a required attribute is null (it shouldn't be).
    /// </summary>
    public class RequiredAttributeViolated : SinglePropertySerializationError
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="property">The property affected by the error at string format</param>
        public RequiredAttributeViolated(string property) : base(property)
        {
        }
    }
}