using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static APITools.Core.Base.DAO.Models.SerializationResultBuilder;

namespace APITools.Core.Base.DAO.Models
{
    /// <summary>
    /// Represents the base class for all classes representing entities that can be saved in a database.
    /// </summary>
    public abstract class Entity : IEquatable<IGuidResolvable>, IGuidResolvable, IValidatable
    {
        private Guid id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected Entity()
        {
            Id = Guid.Empty;
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
        public virtual SerializationResult CanBeDeleted()
        {
            return Ok(this);
        }

        /// <inheritdoc cref="IValidatable.CanBeSavedOrUpdated"/>
        public virtual SerializationResult CanBeSavedOrUpdated()
        {
            return Ok(this);
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
    }
}