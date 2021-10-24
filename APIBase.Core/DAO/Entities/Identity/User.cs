using APIBase.Core.DAO.Models;
using APIBase.Core.DAO.SerializationErrors;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIBase.Core.DAO.Entities.Identity
{
    /// <summary>
    /// Represents a user in the identity system, adapted to to the rest of the components.
    /// </summary>
    public class User : IdentityUser<Guid>, IAdaptedEntity<User>
    {
        /// <summary>
        /// Create a new instance.
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
        public virtual bool CanBeDeleted()
        {
            return true;
        }

        /// <inheritdoc cref="IValidatable.CanBeSavedOrUpdated"/>
        public virtual bool CanBeSavedOrUpdated()
        {
            return true;
        }

        /// <inheritdoc cref="Entity.Equals(IGuidResolvable)"/>
        public bool Equals(IGuidResolvable other)
        {
            return Entity.AreEqual(this, other);
        }

        /// <inheritdoc cref="IValidatable.SetSerializationResultOnError(SerializationError)"/>
        public void SetSerializationResultOnError(SerializationError error)
        {
            Adapter.SetSerializationResultOnError(error);
        }

        /// <inheritdoc cref="IValidatable.SetSerializationResultOnSuccess"/>
        public void SetSerializationResultOnSuccess()
        {
            Adapter.SetSerializationResultOnSuccess();
        }
    }
}