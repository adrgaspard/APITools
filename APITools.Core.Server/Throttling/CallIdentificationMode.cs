using System;

namespace APITools.Core.Server.Throttling
{
    /// <summary>
    /// Represents the different ways to identify a client (for the throttling system).$
    /// Can be combined with the "|" operator to create composites identification modes.
    /// </summary>
    [Flags]
    public enum CallIdentificationMode
    {
        /// <summary>
        /// Represents the absence of an identification mode for an api call.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents the identification mode with the session identity token.
        /// </summary>
        SessionIdentity = 1,

        /// <summary>
        /// Represents the identification mode with remote IP address.
        /// </summary>
        IpAddress = 2,
    }
}