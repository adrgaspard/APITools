using APITools.Core.DAO.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace APITools.DAO.Context
{
    /// <summary>
    /// Represents the base class for the Entity Framework Core context compatible with Identity (and all other components of the package).
    /// </summary>
    /// <typeparam name="TUser">The type used to represent a user</typeparam>
    /// <typeparam name="TRole">The type used to represent a role</typeparam>
    public abstract class IndentityCompatibleDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, Guid> where TUser : User where TRole : Role
    {
    }
}