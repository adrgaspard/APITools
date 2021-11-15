using APITools.Core.Base.DAO.Models;
using APITools.Core.Base.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using static APITools.Core.Base.DAO.Models.SerializationResultBuilder;

namespace APITools.Core.Base.DAO.Entities.Identity
{
    /// <summary>
    /// Represents a user token in the identity system, adapted to to the rest of the components.
    /// </summary>
    [Index(nameof(ComputedId), IsUnique = true)]
    public class UserToken : IdentityUserToken<Guid>, IAdaptedEntity<UserToken>
    {
        private static readonly HashAlgorithm _hashAlgorithm = SHA512.Create();
        private Guid _id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public UserToken()
        {
            _id = Guid.Empty;
        }

        /// <inheritdoc cref="IdentityUserToken{TKey}.UserId"/>
        public override Guid UserId
        {
            get => base.UserId;
            set
            {
                base.UserId = value;
                ComputesId();
            }
        }

        /// <inheritdoc cref="IdentityUserToken{TKey}.Name"/>
        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                ComputesId();
            }
        }

        /// <inheritdoc cref="IdentityUserToken{TKey}.LoginProvider"/>
        public override string LoginProvider
        {
            get => base.LoginProvider;
            set
            {
                base.LoginProvider = value;
                ComputesId();
            }
        }

        /// <summary>
        /// Gets or sets the computed global unique identifier of the entity.
        /// </summary>
        /// <remarks>
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
        public Guid Id => ComputedId;

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
            byte[] userId = UserId.ToByteArray();
            byte[] name = Encoding.UTF8.GetBytes(Name ?? "");
            byte[] loginProvider = Encoding.UTF8.GetBytes(LoginProvider ?? "");
            byte[] concatened = new byte[userId.Length + name.Length + loginProvider.Length];
            Buffer.BlockCopy(userId, 0, concatened, 0, userId.Length);
            Buffer.BlockCopy(name, 0, concatened, userId.Length, name.Length);
            Buffer.BlockCopy(loginProvider, 0, concatened, userId.Length + name.Length, name.Length);
            _id = GuidExtensions.HashIntoGuid(concatened, _hashAlgorithm);
        }
    }
}
