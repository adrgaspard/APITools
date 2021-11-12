using System;

namespace APITools.Core.Base.DAO.Models
{
    /// <summary>
    /// Represents all classes that have an editable global unique identifier.
    /// </summary>
    public interface IGuidWriteable
    {
        /// <summary>
        /// Sets the global unique identifier of the object.
        /// </summary>
        Guid Id { set; }
    }
}