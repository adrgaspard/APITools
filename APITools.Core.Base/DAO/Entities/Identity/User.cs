using APITools.Core.Base.DAO.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static APITools.Core.Base.DAO.Models.SerializationResultBuilder;

namespace APITools.Core.Base.DAO.Entities.Identity
{
    /// <summary>
    /// Represents a user in the identity system, adapted to to the rest of the components.
    /// </summary>
    public class User : IdentityUser<Guid>, IAdaptedEntity<User>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public User()
        {
            Id = Guid.Empty;
        }

        /// <inheritdoc cref="IAdaptedEntity{TAdapted}.ComputedId"/>
        [NotMapped]
        public Guid ComputedId => Id;

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

        /// <inheritdoc cref="IAdaptedEntity{TAdapted}.ComputesId"/>
        public void ComputesId()
        {
        }

        /// <inheritdoc cref="Entity.Equals(IGuidResolvable)"/>
        public bool Equals(IGuidResolvable other)
        {
            return Entity.AreEqual(this, other);
        }
    }
}