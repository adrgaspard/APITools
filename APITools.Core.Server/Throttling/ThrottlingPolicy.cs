using System;

namespace APITools.Core.Server.Throttling
{
    /// <summary>
    /// Represents a set of throttling rules.
    /// </summary>
    public struct ThrottlingPolicy : IEquatable<ThrottlingPolicy>
    {
        /// <summary>
        /// Gets or sets the delay of memorization of a call.
        /// </summary>
        public int CallMemorizationDelay { get; init; }

        /// <summary>
        /// Gets or sets the maximum number of calls permitted before a rejection.
        /// </summary>
        public int MaxCallsBeforeReject { get; init; }

        /// <summary>
        /// Gets or sets the penalty duration on rejection when the client sent too many requests.
        /// </summary>
        public int RejectionPenaltyDelay { get; init; }

        /// <summary>
        /// Checks if two throttling policies are not equal.
        /// </summary>
        /// <param name="left">The first throttling policy</param>
        /// <param name="right">The second throttling policy</param>
        /// <returns>A value that indicates whether the throttling policies are not equal</returns>
        public static bool operator !=(ThrottlingPolicy left, ThrottlingPolicy right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Checks if two throttling policies are equal.
        /// </summary>
        /// <param name="left">The first throttling policy</param>
        /// <param name="right">The second throttling policy</param>
        /// <returns>A value that indicates whether the throttling policies are equal</returns>
        public static bool operator ==(ThrottlingPolicy left, ThrottlingPolicy right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if the throttling policy is equal to another throttling policy.
        /// </summary>
        /// <param name="other">The other throttling policy to be compared</param>
        /// <returns>A value that indicates whether both throttling policies are equal</returns>
        public bool Equals(ThrottlingPolicy other)
        {
            return MaxCallsBeforeReject == other.MaxCallsBeforeReject && CallMemorizationDelay == other.CallMemorizationDelay && RejectionPenaltyDelay == other.RejectionPenaltyDelay;
        }

        /// <summary>
        /// Checks if the throttling policy is equal to another object.
        /// </summary>
        /// <param name="obj">The other object to be compared</param>
        /// <returns>A value that indicates whether the throttling policy equals to the other object</returns>
        public override bool Equals(object obj)
        {
            return obj is ThrottlingPolicy policy && Equals(policy);
        }

        /// <summary>
        /// Gets the hash of the throttling policy.
        /// </summary>
        /// <returns>The hashed value of the throttling policy</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(MaxCallsBeforeReject, CallMemorizationDelay, RejectionPenaltyDelay);
        }
    }
}