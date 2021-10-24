using APIBase.Core.DAO.SerializationErrors;
using System;
using System.Text.Json.Serialization;

namespace APIBase.Core.DAO.Models
{
    /// <summary>
    /// Represents a result of a test or a serialization test on an entity.
    /// </summary>
    /// <see cref="Entity"/>
    public class SerializationResult
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="sender">The entity that performed a serialization trial or test</param>
        /// <param name="error">The specified error of the result (set to null if the test/attempt was success)</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="sender"/> is null</exception>
        protected internal SerializationResult(Entity sender, SerializationError error)
        {
            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            Sender = sender;
            Error = error;
        }

        /// <summary>
        /// Gets or sets the specified error of the result.
        /// It can be null.
        /// </summary>
        public SerializationError Error { get; init; }

        /// <summary>
        /// Gets a value that indicates whether the result is a success or not (an error).
        /// </summary>
        public bool IsOk => Error is null;

        /// <summary>
        /// Gets or sets the entity that performed a serialization trial or test.
        /// </summary>
        [JsonIgnore]
        public Entity Sender { get; init; }

        /// <summary>
        /// Gets the identifier of the entity that performed a serialization trial or test.
        /// </summary>
        public Guid SenderId => Sender.Id;

        /// <summary>
        /// Gets the type at string format of the entity that performed a serialization trial or test.
        /// </summary>
        public string SenderType => Sender.GetType().Name;
    }
}