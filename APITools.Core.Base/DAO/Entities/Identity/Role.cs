using APITools.Core.Base.DAO.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APITools.Core.Base.DAO.Entities.Identity
{
    /// <summary>
    /// Represents a role in the identity system, adapted to to the rest of the components.
    /// </summary>
    public class Role : IdentityRole<Guid>, IAdaptedEntity<Role>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public Role()
        {
            Adapter = new EntityAdapter<Role>(this);
        }

        /// <inheritdoc cref="IAdaptedEntity{TAdapted}.Adapter"/>
        [JsonIgnore]
        [NotMapped]
        public EntityAdapter<Role> Adapter { get; protected init; }

        /// <inheritdoc cref="IValidatable.CanBeDeleted"/>
        public virtual SerializationResult CanBeDeleted()
        {
            return new(this, null);
        }

        /// <inheritdoc cref="IValidatable.CanBeSavedOrUpdated"/>
        public virtual SerializationResult CanBeSavedOrUpdated()
        {
            return new(this, null);
        }

        /// <inheritdoc cref="Entity.Equals(IGuidResolvable)"/>
        public bool Equals(IGuidResolvable other)
        {
            return Entity.AreEqual(this, other);
        }
    }
}