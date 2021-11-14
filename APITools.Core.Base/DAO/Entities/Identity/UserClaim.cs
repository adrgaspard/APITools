using APITools.Core.Base.DAO.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static APITools.Core.Base.DAO.Models.SerializationResultBuilder;

namespace APITools.Core.Base.DAO.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<Guid>, IAdaptedEntity<UserClaim>
    {
        private Guid _id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public UserClaim()
        {
            Adapter = new EntityAdapter<UserClaim>(this);
        }

        /// <inheritdoc cref="IdentityUserClaim{TKey}.Id"/>
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
        public EntityAdapter<UserClaim> Adapter { get; protected init; }

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
