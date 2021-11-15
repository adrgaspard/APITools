using APITools.Core.Base.DAO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static APITools.Core.Base.DAO.Models.SerializationResultBuilder;

namespace APITools.Core.Base.DAO.Entities.Identity
{
    /// <summary>
    /// Represents a user claim in the identity system, adapted to to the rest of the components.
    /// </summary>
    [Index(nameof(ComputedId), IsUnique = true)]
    public class UserClaim : IdentityUserClaim<Guid>, IAdaptedEntity<UserClaim>
    {
        private Guid _id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public UserClaim()
        {
            _id = Guid.Empty;
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
        /// Gets or sets the computed global unique identifier of the entity.
        /// </summary>
        /// <remarks>
        /// If the implementation has a mapped "Id" property of another type than Guid, the IGuidResolvable.Id property must be not mapped and must redirect on this one. Also, a unique must be set on this property.
        /// Else, this property must be not mapped and redirects on IGuidResolvable.Id.
        /// In all cases, this property and the IGuidResolvable property must always have the same value.
        /// </remarks>
        public Guid ComputedId
        {
            get => _id;
            set
            {
                if (_id != Guid.Empty)
                {
                    throw new NotSupportedException("The Id of the entity can't be modified on it's Guid form");
                }
                _id = value;
            }
        }

        /// <summary>
        /// Gets the global unique identifier of the entity.
        /// </summary>
        /// <remarks>Redirects on the computed id</remarks>
        [NotMapped]
        Guid IGuidResolvable.Id => ComputedId;

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

        /// <inheritdoc cref="IAdaptedEntity{TAdapted}.ComputesId"/>
        public void ComputesId()
        {
        }
    }
}
