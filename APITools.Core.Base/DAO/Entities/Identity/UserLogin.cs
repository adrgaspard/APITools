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
    [Index(nameof(ComputedId), IsUnique = true)]
    public class UserLogin : IdentityUserLogin<Guid>, IAdaptedEntity<UserLogin>
    {
        private static readonly HashAlgorithm _hashAlgorithm = SHA512.Create();
        private Guid _id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public UserLogin()
        {
            _id = Guid.Empty;
        }

        /// <inheritdoc cref="IdentityUserLogin{TKey}.ProviderKey"/>
        public override string ProviderKey
        {
            get => base.ProviderKey;
            set
            {
                base.ProviderKey = value;
                ComputesId();
            }
        }

        /// <inheritdoc cref="IdentityUserLogin{TKey}.LoginProvider"/>
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
            byte[] loginProvider = Encoding.UTF8.GetBytes(LoginProvider ?? "");
            byte[] providerKey = Encoding.UTF8.GetBytes(ProviderKey ?? "");
            byte[] concatened = new byte[loginProvider.Length + providerKey.Length];
            Buffer.BlockCopy(loginProvider, 0, concatened, 0, loginProvider.Length);
            Buffer.BlockCopy(providerKey, 0, concatened, loginProvider.Length, providerKey.Length);
            _id = GuidExtensions.HashIntoGuid(concatened, _hashAlgorithm);
        }
    }
}
