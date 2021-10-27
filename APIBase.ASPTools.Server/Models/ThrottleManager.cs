using APIBase.Core.Throttling;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace APIBase.ASPTools.Server.Models
{
    /// <summary>
    /// Memory-cache-based implementation for throttling management.
    /// </summary>
    public class ThrottleManager : IThrottleManager
    {
        /// <summary>
        /// Gets or sets the cache used to store the sessions identities.
        /// </summary>
        protected MemoryCache SessionIdentityCache { get; init; }

        /// <summary>
        /// Gets or sets the cache used to store the ip addresses.
        /// </summary>
        protected MemoryCache IpAddressCache { get; init; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ThrottleManager()
        {
            SessionIdentityCache = new(new MemoryCacheOptions());
            IpAddressCache = new(new MemoryCacheOptions());
        }

        /// <inheritdoc cref="IThrottleManager.ValidateThrottleOnCall(CallInfo, CallIdentificationMode)"/>
        public bool ValidateThrottleOnCall(CallInfo call, CallIdentificationMode identificationMode)
        {
            bool isOk = true;

            if ((CallIdentificationMode.SessionIdentity & identificationMode) == CallIdentificationMode.SessionIdentity)
            {
                isOk &= ValidateCallForMemoryCache(call, $"{call.Route}-{call.SessionIdentity}", SessionIdentityCache);
            }
            if ((CallIdentificationMode.IpAddress & identificationMode) == CallIdentificationMode.IpAddress)
            {
                isOk &= ValidateCallForMemoryCache(call, $"{call.Route}-{call.Address}", IpAddressCache);
            }
            return isOk;
        }

        /// <summary>
        /// Checks a call for a given cache and makes the necessary changes in the cache.
        /// </summary>
        /// <param name="call">The call to be checked</param>
        /// <param name="callIdentifier">The identifier of the call (used as a key in the cache)</param>
        /// <param name="cache">The cache to manage</param>
        /// <returns></returns>
        protected static bool ValidateCallForMemoryCache(CallInfo call, string callIdentifier, IMemoryCache cache)
        {
            int nbCalls = 1;
            if (cache.TryGetValue(callIdentifier, out int previewRequestCount))
            {
                nbCalls += previewRequestCount;
                if (previewRequestCount >= call.ThrottlingPolicy.MaxCallsBeforeReject)
                {
                    cache.Set(callIdentifier, nbCalls, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(call.ThrottlingPolicy.RejectionPenaltyDelay)));
                    return false;
                }
            }
            cache.Set(callIdentifier, nbCalls, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(call.ThrottlingPolicy.CallMemorizationDelay)));
            return true;
        }
    }
}