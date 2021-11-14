using APITools.Core.Base.DAO.Models;
using APITools.Core.Base.Tools;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using static APITools.Core.Base.DAO.Models.SerializationResultBuilder;

namespace APITools.Core.Base.DAO.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<Guid>, IAdaptedEntity<UserLogin>
    {
        private static readonly HashAlgorithm _hashAlgorithm = SHA512.Create();
        private Guid _id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public UserLogin()
        {
            Adapter = new EntityAdapter<UserLogin>(this);
            EditId();
        }

        /// <inheritdoc cref="IdentityUserLogin{TKey}.ProviderKey"/>
        public override string ProviderKey
        {
            get => base.ProviderKey;
            set
            {
                base.ProviderKey = value;
                EditId();
            }
        }

        /// <inheritdoc cref="IdentityUserLogin{TKey}.LoginProvider"/>
        public override string LoginProvider
        {
            get => base.LoginProvider;
            set
            {
                base.LoginProvider = value;
                EditId();
            }
        }

        /// <summary>
        /// Sets the global unique identifier of the entity.
        /// </summary>
        /// <exception cref="NotSupportedException">Occurs when the setter is called</exception>
        public Guid Id
        {
            get => _id;
            set => throw new NotSupportedException("The Id of the entity can't be modified on it's Guid form");
        }

        /// <inheritdoc cref="IAdaptedEntity{TAdapted}.Adapter"/>
        [JsonIgnore]
        [NotMapped]
        public EntityAdapter<UserLogin> Adapter { get; protected init; }

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

        /// <summary>
        /// Computes and changes the id of the entity.
        /// </summary>
        protected virtual void EditId()
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
