using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace APIBase.Core.DAO.SerializationErrors
{
    /// <summary>
    /// Represents the base class for all classes that represents errors that relate to many specific properties during serialization tests.
    /// </summary>
    public abstract class MultiPropertySerializationError : SerializationError
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="properties">The list of the properties affected by the error at string format</param>
        /// <exception cref="ArgumentException">Occurs when the list is null or empty, or when at least one property name is null, empty, or consists only of white-space characters</exception>
        protected MultiPropertySerializationError(IEnumerable<string> properties)
        {
            if (properties is null || !properties.Any())
            {
                throw new ArgumentException("At least one property must be declared");
            }
            int i = 0;
            foreach (string property in properties)
            {
                if (string.IsNullOrWhiteSpace(property))
                {
                    throw new ArgumentException($"The property at index {i} must have a valid name");
                }
                i++;
            }
            Properties = new ReadOnlyCollection<string>(new List<string>(properties));
        }

        /// <summary>
        /// Gets or sets the list of the properties affected by the error at string format.
        /// </summary>
        public IList<string> Properties { get; init; }
    }
}