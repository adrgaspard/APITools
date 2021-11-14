using APITools.Core.Base.DAO.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APITools.Core.Base.DAO.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<Guid>, IAdaptedEntity<RoleClaim>
    {
        private Guid _id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public RoleClaim()
        {
            Adapter = new EntityAdapter<RoleClaim>(this);
        }

        /// <inheritdoc cref="IdentityRoleClaim{TKey}.Id"/>
        public override int Id
        {
            get => base.Id;
            set
            {
                base.Id = value;
                byte[] bytes = new byte[16];
                BitConverter.GetBytes(value).CopyTo(bytes, 0);
                _id = new Guid(bytes);
            }
        }

        /// <summary>
        /// Gets the global unique identifier of the entity.
        /// </summary>
        Guid IGuidResolvable.Id => _id;

        /// <summary>
        /// Sets the global unique identifier of the entity.
        /// </summary>
        /// <exception cref="NotSupportedException">Occurs when called</exception>
        Guid IGuidWriteable.Id { set => throw new NotSupportedException("The Id of the entity can't be modified on it's Guid form"); }

        /// <inheritdoc cref="IAdaptedEntity{TAdapted}.Adapter"/>
        [JsonIgnore]
        [NotMapped]
        public EntityAdapter<RoleClaim> Adapter { get; protected init; }

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
