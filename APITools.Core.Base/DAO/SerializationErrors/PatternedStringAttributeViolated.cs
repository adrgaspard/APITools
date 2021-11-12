namespace APITools.Core.Base.DAO.SerializationErrors
{
    /// <summary>
    /// Represents the serialization error when a string attribute don't match with a regex when it should be.
    /// </summary>
    public class PatternedStringAttributeViolated : SinglePropertySerializationError
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="property">The property affected by the error at string format</param>
        /// <param name="regexViolated">The regex that should match with the property value but don't</param>
        public PatternedStringAttributeViolated(string property, string regexViolated) : base(property)
        {
            RegexViolated = regexViolated;
        }

        /// <summary>
        /// Gets or sets tThe property affected by the error at string format.
        /// </summary>
        public string RegexViolated { get; init; }
    }
}