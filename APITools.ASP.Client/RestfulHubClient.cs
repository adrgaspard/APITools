using APITools.ASP.Base;
using APITools.ASP.Base.C2SMessages;
using APITools.ASP.Base.S2CMessages;
using APITools.Core.Base.ComponentModel;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace APITools.ASP.Client
{
    public class RestfulHubClient<TEntity> : HubClient, INotifyItemOperationPerformed<Guid>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="hubConnectionBuilder">The connection builder (with its parameters)</param>
        /// <param name="immediatelyStart">Whether the connection starts immediately after being built</param>
        /// <param name="subscription"></param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="hubConnectionBuilder"/> is null</exception>
        public RestfulHubClient(HubConnectionBuilder hubConnectionBuilder, bool immediatelyStart, ISubscription subscription) : base(hubConnectionBuilder, immediatelyStart)
        {
            Subscription = new PermanentSubscription();
            Subscription.SetValuesFrom(subscription);
            Subscription.PropertyChanged += OnSubscriptionChanged;
            Connection.On<SetItemOperationPerformedNotification>(BaseHubEndpoints.NotifySetItemOperationPerformedClientEndpoint, (notification) => ItemOperationPerformed?.Invoke(this, new SetItemOperationPerformedEventArgs<Guid>(notification.OperationType, notification.ImpactedItemsIds)));
        }

        /// <inheritdoc cref="INotifyItemOperationPerformed{TItemId}.ItemOperationPerformed"/>
        public event EventHandler<SetItemOperationPerformedEventArgs<Guid>> ItemOperationPerformed;

        /// <summary>
        /// Gets or sets the subscription of the client.
        /// </summary>
        public PermanentSubscription Subscription { get; private init; }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            base.Dispose();
            Subscription.PropertyChanged -= OnSubscriptionChanged;
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="HubClient.OnDisconnected(Exception)"/>
        protected override Task OnDisconnected(Exception disconnectionException)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="HubClient.OnReconnected(string)"/>
        protected override Task OnReconnected(string connectionId)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref="HubClient.OnReconnecting(Exception)"/>
        protected override Task OnReconnecting(Exception disconnectionException)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when the client subscription has changed.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="eventArgs">The arguments of the event</param>
        protected void OnSubscriptionChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            Connection.SendAsync(BaseHubEndpoints.UpdateSubscriptionServerEndpoint, new UpdateSubscriptionRequest(Subscription));
        }
    }
}