using APITools.Core.Base.DAO.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace APITools.DAO.Context
{
    /// <summary>
    /// Represents the base class for the Entity Framework Core context compatible with Identity (and all other components of the package) with the role management components.
    /// </summary>
    /// <typeparam name="TUser">The type used to represent a user</typeparam>
    /// <typeparam name="TRole">The type used to represent a role</typeparam>
    /// <typeparam name="TUserClaim">The type used to represent a user claim</typeparam>
    /// <typeparam name="TUserRole">The type used to represent a user role</typeparam>
    /// <typeparam name="TUserLogin">The type used to represent a user login</typeparam>
    /// <typeparam name="TRoleClaim">The type used to represent a role claim</typeparam>
    /// <typeparam name="TUserToken">The type used to represent a user token</typeparam>
    public abstract class IdentityCompatibleDbContext<TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : IdentityDbContext<TUser, TRole, Guid, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> where TUser : User where TRole : Role where TUserClaim : UserClaim where TUserRole : UserRole where TUserLogin : UserLogin where TRoleClaim : RoleClaim where TUserToken : UserToken
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public IdentityCompatibleDbContext() : base()
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext</param>
        public IdentityCompatibleDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    /// <summary>
    /// Represents the base class for the Entity Framework Core context compatible with Identity (and all other components of the package) with the role management components.
    /// </summary>
    /// <typeparam name="TUser">The type used to represent a user</typeparam>
    /// <typeparam name="TRole">The type used to represent a role</typeparam>
    public abstract class IdentityCompatibleDbContext<TUser, TRole> : IdentityCompatibleDbContext<TUser, TRole, UserClaim, UserRole, UserLogin, RoleClaim, UserToken> where TUser : User where TRole : Role
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public IdentityCompatibleDbContext() : base()
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext</param>
        public IdentityCompatibleDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    /// <summary>
    /// Represents the base class for the Entity Framework Core context compatible with Identity (and all other components of the package) with the role management components.
    /// </summary>
    public abstract class IdentityCompatibleDbContext : IdentityCompatibleDbContext<User, Role>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public IdentityCompatibleDbContext() : base()
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext</param>
        public IdentityCompatibleDbContext(DbContextOptions options) : base(options)
        {
        }
    }

}