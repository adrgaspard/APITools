namespace APITools.Core.DAO.SerializationErrors
{
    /// <summary>
    /// Represents the base class for all classes that represents errors during serialization tests.
    /// </summary>
    public abstract class SerializationError
    {
        /// <summary>
        /// Gets the type of the error at string format.
        /// </summary>
        public string Type => GetType().Name;
    }
}