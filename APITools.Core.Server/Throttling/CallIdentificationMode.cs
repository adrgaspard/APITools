using System;

namespace APITools.Core.Server.Throttling
{
    /// <summary>
    /// Represents the different ways to identify a client (for the throttling system).
    /// </summary>
    [Flags]
    public enum CallIdentificationMode
    {
        None = 0,
        SessionIdentity = 1,
        IpAddress = 2,
    }
}