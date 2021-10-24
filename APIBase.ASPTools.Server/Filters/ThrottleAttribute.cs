﻿using APIBase.Core.Throttling;
using CommonServiceLocator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Threading.Tasks;

namespace APIBase.ASPTools.Server.Filters
{
    /// <summary>
    /// Represents a throttling policy assignation for a route.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ThrottleAttribute : Attribute, IAsyncActionFilter
    {
        /// <summary>
        /// Gets or sets the route concerned by the throttling policy.
        /// </summary>
        public string Route { get; protected init; }

        /// <summary>
        /// Gets or sets the throttling policy.
        /// </summary>
        public ThrottlingPolicy ThrottlingPolicy { get; protected init; }

        /// <summary>
        /// Gets or sets the identification mode to identify the client.
        /// </summary>
        public CallIdentificationMode IdentificationMode { get; protected init; }

        /// <summary>
        /// Gets or sets the throttle manager, that validates (or not) a call.
        /// </summary>
        protected IThrottleManager ThrottleManager { get; set; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="route">The route concerned by the throttling policy</param>
        /// <param name="maxCallsBeforeReject">The number of possible calls from the endpoint before being rejected</param>
        /// <param name="callMemorizationDelay">The duration of a call from the endpoint</param>
        /// <param name="rejectionPenaltyDelay">The duration of the rejection penalty for frequent calls</param>
        /// <param name="identificationMode">The identification mode to identify the client</param>
        public ThrottleAttribute(string route, int maxCallsBeforeReject, int callMemorizationDelay, int rejectionPenaltyDelay, CallIdentificationMode identificationMode)
        {
            Route = route;
            ThrottlingPolicy = new ThrottlingPolicy { MaxCallsBeforeReject = maxCallsBeforeReject, CallMemorizationDelay = callMemorizationDelay, RejectionPenaltyDelay = rejectionPenaltyDelay };
            IdentificationMode = identificationMode;
        }

        /// <inheritdoc cref="IAsyncActionFilter.OnActionExecutionAsync(ActionExecutingContext, ActionExecutionDelegate)"/>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (ThrottleManager is null)
            {
                ThrottleManager = ServiceLocator.Current.GetInstance<IThrottleManager>();
            }
            CallInfo call = new() { ThrottlingPolicy = ThrottlingPolicy, Route = Route, Address = context.HttpContext.Response.HttpContext.Connection.RemoteIpAddress, SessionIdentity = context.HttpContext.Request.HttpContext.Connection.Id };
            if (!ThrottleManager.ValidateThrottleOnCall(call, IdentificationMode))
            {
                context.Result = new ContentResult
                {
                    Content = $"Request limit is exceeded. Try again in {ThrottlingPolicy.RejectionPenaltyDelay} seconds.",
                    StatusCode = (int)HttpStatusCode.TooManyRequests
                };
                return;
            }
            await next();
        }
    }
}