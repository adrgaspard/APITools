using System;

namespace APIBase.Core.DAO.Models
{
    /// <summary>
    /// Represents all classes that have an global unique identifier.
    /// </summary>
    public interface IGuidResolvable
    {
        /// <summary>
        /// Gets the global unique identifier of the object.
        /// </summary>
        Guid Id { get; }
    }
}