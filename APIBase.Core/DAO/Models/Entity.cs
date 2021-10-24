using APIBase.Core.DAO.SerializationErrors;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIBase.Core.DAO.Models
{
    /// <summary>
    /// Represents the base class for all classes representing entities that can be saved in a database.
    /// </summary>
    public abstract class Entity : IEquatable<IGuidResolvable>, IGuidResolvable, IValidatable
    {
        private Guid id;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        protected Entity()
        {
            Id = Guid.Empty;
            SerializationResult = null;
        }

        /// <summary>
        /// Gets or sets the global unique identifier of the entity.
        /// </summary>
        /// <exception cref="InvalidOperationException">Occurs when the property is redefined when it already has a value other than an empty Guid</exception>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id
        {
            get => id;
            set
            {
                if (id == Guid.Empty)
                {
                    id = value;
                }
                else
                {
                    throw new InvalidOperationException("The Id of an Entity can't be edited");
                }
            }
        }

        /// <summary>
        /// Gets or sets the serialization result of the entity.
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        public SerializationResult SerializationResult { get; protected set; }

        /// <summary>
        /// Checks if two objects with an identifier are equal (the comparison will be made on their respective identifiers).
        /// </summary>
        /// <param name="left">The first object to compare</param>
        /// <param name="right">The second object to compare</param>
        /// <returns>A value that indicates whether the two objects are equal</returns>
        public static bool AreEqual(IGuidResolvable left, IGuidResolvable right)
        {
            if (left is null || right is null)
            {
                return left is null && right is null;
            }
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (left.Id.Equals(Guid.Empty) || right.Id.Equals(Guid.Empty))
            {
                return false;
            }
            return left.Id == right.Id && left.GetType() == right.GetType();
        }

        /// <inheritdoc cref="IValidatable.CanBeDeleted"/>
        public virtual bool CanBeDeleted()
        {
            return true;
        }

        /// <inheritdoc cref="IValidatable.CanBeSavedOrUpdated"/>
        public virtual bool CanBeSavedOrUpdated()
        {
            return true;
        }

        /// <summary>
        /// Checks if the entity is equal to another object with an identifier (the comparison will be made on their respective identifiers).
        /// </summary>
        /// <param name="other">The other object to compare</param>
        /// <returns>A value that indicates whether the entity and the other objects are equal</returns>
        public bool Equals(IGuidResolvable other)
        {
            return AreEqual(this, other);
        }

        /// <summary>
        /// Checks if the entity is equal to another object.
        /// </summary>
        /// <param name="obj">The other object to compare</param>
        /// <returns>A value that indicates whether the entity and the other object are equal</returns>
        public override bool Equals(object obj)
        {
            return obj is IGuidResolvable other && Equals(other);
        }

        /// <summary>
        /// Compute the hashcode of the entity, based on its identifier.
        /// </summary>
        /// <returns>The hash of the entity</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        /// <inheritdoc cref="IValidatable.SetSerializationResultOnError(SerializationError)"/>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="error"/> is null</exception>
        public void SetSerializationResultOnError(SerializationError error)
        {
            if (error is null)
            {
                throw new ArgumentNullException(nameof(error));
            }
            SerializationResult = new SerializationResult(this, error);
        }

        /// <inheritdoc cref="IValidatable.SetSerializationResultOnSuccess"/>
        public void SetSerializationResultOnSuccess()
        {
            SerializationResult = new SerializationResult(this, null);
        }
    }
}