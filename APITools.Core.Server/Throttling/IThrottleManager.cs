namespace APITools.Core.Server.Throttling
{
    /// <summary>
    /// Represents all classes that validates calls with the throttling aspect.
    /// </summary>
    public interface IThrottleManager
    {
        /// <summary>
        /// Checks if a call is valid from a throttling point of view.
        /// </summary>
        /// <param name="call">The call to be checked</param>
        /// <param name="identificationMode">The identification mode</param>
        /// <returns>A value that indicates if the call respects the throttling or not</returns>
        bool ValidateThrottleOnCall(CallInfo call, CallIdentificationMode identificationMode);
    }
}