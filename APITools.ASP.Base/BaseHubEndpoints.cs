namespace APITools.ASP.Base
{
    /// <summary>
    /// Classe that stores all the base hub endpoints names.
    /// </summary>
    public static class BaseHubEndpoints
    {
        /// <summary>
        /// Client endpoint : the server can call it to notify a client that some items have changed.
        /// </summary>
        public const string NotifySetItemOperationPerformedClientEndpoint = "NotifySetItemOperationPerformed";

        /// <summary>
        /// Server endpoint : a client can call it to change his subscription to items events.
        /// </summary>
        public const string UpdateSubscriptionServerEndpoint = "UpdateSubscription";
    }
}