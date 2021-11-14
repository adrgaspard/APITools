using APITools.Core.Base.DAO.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
            Adapter = new EntityAdapter<User>(this);
        }

        /// <inheritdoc cref="IAdaptedEntity{TAdapted}.Adapter"/>
        [JsonIgnore]
        [NotMapped]
        public EntityAdapter<User> Adapter { get; protected init; }

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

        /// <inheritdoc cref="Entity.Equals(IGuidResolvable)"/>
        public bool Equals(IGuidResolvable other)
        {
            return Entity.AreEqual(this, other);
        }
    }
}