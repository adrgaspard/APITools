using System;

namespace APIBase.Core.ComponentModel
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