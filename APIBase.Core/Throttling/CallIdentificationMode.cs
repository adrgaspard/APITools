using System;

namespace APIBase.Core.Throttling
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