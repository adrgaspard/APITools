using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace APITools.Core.Server.Throttling
{
    /// <summary>
    /// Represents a call on an endpoint by a client (with the endpoint throttling policy).
    /// </summary>
    public class CallInfo : IEquatable<CallInfo>
    {
        /// <summary>
        /// Gets or sets the ip address of client.
        /// </summary>
        public IPAddress Address { get; init; }

        /// <summary>
        /// Gets or sets the route of the endpoint.
        /// </summary>
        public string Route { get; init; }

        /// <summary>
        /// Gets or sets the client session unique identifier.
        /// </summary>
        public string SessionIdentity { get; init; }

        /// <summary>
        /// Gets or sets the used throttling policy for the call.
        /// </summary>
        public ThrottlingPolicy ThrottlingPolicy { get; init; }

        /// <summary>
        /// Checks if the call is equal to another one.
        /// </summary>
        /// <param name="other">The other call to be compared</param>
        /// <returns>A value that indicates whether the both calls are equal</returns>
        public bool Equals([AllowNull] CallInfo other)
        {
            return other is not null && Route == other.Route && SessionIdentity == other.SessionIdentity && Address.Equals(other.Address);
        }

        /// <summary>
        /// Checks if the call is equal to another object.
        /// </summary>
        /// <param name="obj">The other object to be compared</param>
        /// <returns>A value that indicates whether the call is equal to the other object</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj is CallInfo call)
            {
                return Equals(call);
            }
            return false;
        }

        /// <summary>
        /// Gets a value that represent the hash of the call.
        /// </summary>
        /// <returns>The hashed value of the call</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(ThrottlingPolicy, Route, SessionIdentity, Address);
        }
    }
}