using APITools.Core.Base.DAO.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace APITools.DAO.Context
{
    /// <summary>
    /// Represents the base class for the Entity Framework Core context compatible with Identity (and all other components of the package) without the role management components.
    /// </summary>
    /// <typeparam name="TUser">The type used to represent a user</typeparam>
    /// <typeparam name="TUserClaim">The type used to represent a user claim</typeparam>
    /// <typeparam name="TUserLogin">The type used to represent a user login</typeparam>
    /// <typeparam name="TUserToken">The type used to represent a user token</typeparam>
    public class IdentityCompatibleUserContext<TUser, TUserClaim, TUserLogin, TUserToken> : IdentityUserContext<TUser, Guid, TUserClaim, TUserLogin, TUserToken> where TUser : User where TUserClaim : UserClaim where TUserLogin : UserLogin where TUserToken : UserToken
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public IdentityCompatibleUserContext() : base()
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext</param>
        public IdentityCompatibleUserContext(DbContextOptions options) : base(options)
        {
        }
    }

    /// <summary>
    /// Represents the base class for the Entity Framework Core context compatible with Identity (and all other components of the package) without the role management components.
    /// </summary>
    /// <typeparam name="TUser">The type used to represent a user</typeparam>
    public class IdentityCompatibleUserContext<TUser> : IdentityCompatibleUserContext<TUser, UserClaim, UserLogin, UserToken> where TUser : User
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public IdentityCompatibleUserContext() : base()
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext</param>
        public IdentityCompatibleUserContext(DbContextOptions options) : base(options)
        {
        }
    }

    /// <summary>
    /// Represents the base class for the Entity Framework Core context compatible with Identity (and all other components of the package) without the role management components.
    /// </summary>
    public class IdentityCompatibleUserContext : IdentityCompatibleUserContext<User>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public IdentityCompatibleUserContext() : base()
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext</param>
        public IdentityCompatibleUserContext(DbContextOptions options) : base(options)
        {
        }
    }
}
