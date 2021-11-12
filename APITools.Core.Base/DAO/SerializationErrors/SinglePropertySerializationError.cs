using System;

namespace APITools.Core.Base.DAO.SerializationErrors
{
    /// <summary>
    /// Represents the base class for all classes that represents errors that relate to a specific property during serialization tests.
    /// </summary>
    public abstract class SinglePropertySerializationError : SerializationError
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="property">The property affected by the error at string format</param>
        /// <exception cref="ArgumentException">Occurs when the property name is null, empty, or consists only of white-space characters</exception>
        protected SinglePropertySerializationError(string property)
        {
            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentException("The property must have a valid name");
            }
            Property = property;
        }

        /// <summary>
        /// Gets or sets the property affected by the error at string format.
        /// </summary>
        public string Property { get; init; }
    }
}