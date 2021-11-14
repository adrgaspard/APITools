using APITools.Core.Base.DAO.SerializationErrors;
using System;

namespace APITools.Core.Base.DAO.Models
{
    public static class SerializationResultBuilder
    {
        /// <summary>
        /// Creates a new serialization result that indicates a success.
        /// </summary>
        /// <param name="sender">The entity that performed a serialization trial or test</param>
        /// <returns>The instanciated serialization result</returns>
        public static SerializationResult Ok(IGuidResolvable sender)
        {
            return new(sender, null);
        }

        /// <summary>
        /// Creates a new serialization result that indicates an error.
        /// </summary>
        /// <param name="sender">The entity that performed a serialization trial or test</param>
        /// <param name="error">The specified error of the result</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="sender"/> or <paramref name="error"/> is null</exception>
        /// <returns>The instanciated serialization result</returns>
        public static SerializationResult Error(IGuidResolvable sender, SerializationError error)
        {
            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            if (error is null)
            {
                throw new ArgumentNullException(nameof(error));
            }
            return new(sender, error);
        }
    }
}
