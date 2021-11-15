using System;

namespace APITools.Core.Base.DAO.Models
{
    /// <summary>
    /// Represents all basic types of operations possibles in a set of unordered items.
    /// Can be combined with the "|" operator to create composites operation types.
    /// </summary>
    [Flags]
    public enum OperationType
    {
        /// <summary>
        /// Represents all read-only operations.
        /// </summary>
        Read = 1,

        /// <summary>
        /// Represents an operation to add one or more items.
        /// </summary>
        Create = 2,

        /// <summary>
        /// Represents an operation to update one or more items.
        /// </summary>
        Update = 4,

        /// <summary>
        /// Represents an operation to delete one or more items.
        /// </summary>
        Delete = 8
    }
}