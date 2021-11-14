using APITools.Core.Base.DAO.Models;
using APITools.Core.Base.Tools;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using static APITools.Core.Base.DAO.Models.SerializationResultBuilder;

namespace APITools.Core.Base.DAO.Entities.Identity
{
    public class UserRole : IdentityUserRole<Guid>, IAdaptedEntity<UserRole>
    {
        private static readonly HashAlgorithm _hashAlgorithm = SHA512.Create();
        private Guid _id;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public UserRole()
        {
            Adapter = new EntityAdapter<UserRole>(this);
        }

        /// <inheritdoc cref="IdentityUserRole{TKey}.UserId"/>
        public override Guid UserId
        {
            get => base.UserId;
            set
            {
                base.UserId = value;
                EditId();
            }
        }

        /// <inheritdoc cref="IdentityUserRole{TKey}.RoleId"/>
        public override Guid RoleId
        {
            get => base.RoleId;
            set
            {
                base.RoleId = value;
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
        public EntityAdapter<UserRole> Adapter { get; protected init; }

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
            byte[] roleId = RoleId.ToByteArray();
            byte[] concatened = new byte[userId.Length + roleId.Length];
            Buffer.BlockCopy(userId, 0, concatened, 0, userId.Length);
            Buffer.BlockCopy(roleId, 0, concatened, userId.Length, roleId.Length);
            _id = GuidExtensions.HashIntoGuid(concatened, _hashAlgorithm);
        }
    }
}
