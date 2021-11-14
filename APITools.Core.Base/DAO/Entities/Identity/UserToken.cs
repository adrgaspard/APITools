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
    public class UserToken : IdentityUserToken<Guid>, IAdaptedEntity<UserToken>
    {
        private static readonly HashAlgorithm _hashAlgorithm = SHA512.Create();
        private Guid _id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public UserToken()
        {
            Adapter = new EntityAdapter<UserToken>(this);
        }

        /// <inheritdoc cref="IdentityUserToken{TKey}.UserId"/>
        public override Guid UserId
        {
            get => base.UserId;
            set
            {
                base.UserId = value;
                EditId();
            }
        }

        /// <inheritdoc cref="IdentityUserToken{TKey}.Name"/>
        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                EditId();
            }
        }

        /// <inheritdoc cref="IdentityUserToken{TKey}.LoginProvider"/>
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
        public EntityAdapter<UserToken> Adapter { get; protected init; }

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
