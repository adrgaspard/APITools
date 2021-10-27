using APIBase.ASPTools.Base;
using APIBase.ASPTools.Base.S2CMessages;
using APIBase.ASPTools.Requests.C2SMessages;
using APIBase.Core.ComponentModel;
using APIBase.Core.DAO.Models;
using APIBase.Core.DAO.Repositories;
using APIBase.Core.Ioc;
using APIBase.Core.Tools;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBase.ASPTools.Server.Hubs
{
    /// <summary>
    /// Represents the base class for the hubs that can notify their clients when a set of items change.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RestfulHub<TEntity> : Hub where TEntity : class, IGuidResolvable, IValidatable
    {
        /// <summary>
        /// The observable repository attribute. Do not use it directely.
        /// </summary>
        private IObservableRepository<TEntity> _repository;

        /// <summary>
        /// Gets or sets the observable repository used by the hub.
        /// </summary>
        protected IObservableRepository<TEntity> Repository
        {
            get => _repository;
            init
            {
                _repository = value;
                _repository.ItemOperationPerformed += Repository_ItemOperationPerformed;
            }
        }

        /// <summary>
        /// Gets or sets the subscriptions to the hub.
        /// </summary>
        protected IDictionary<string, (IClientProxy, PermanentSubscription)> Subscriptions { get; init; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected RestfulHub() : this(true)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="automaticallySetRepositoryWithIoc">Whether the hub repository is to be automatically searched with the service locator or not</param>
        protected RestfulHub(bool automaticallySetRepositoryWithIoc)
        {
            Subscriptions = new Dictionary<string, (IClientProxy, PermanentSubscription)>();
            if (automaticallySetRepositoryWithIoc)
            {
                Repository = ServiceLocator.Current.GetInstance<IObservableRepository<TEntity>>();
            }
        }

        /// <summary>
        /// Server endpoint : a client can call it to change his subscription to items events.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HubMethodName(BaseHubEndpoints.UpdateSubscriptionServerEndpoint)]
        public virtual async Task UpdateSubscription(UpdateSubscriptionRequest request)
        {
            await Task.Run(() =>
            {
                if (request.IsEmpty)
                {
                    Subscriptions.Remove(Context.ConnectionId);
                    return;
                }
                if (Subscriptions.TryGetValue(Context.ConnectionId, out (IClientProxy, PermanentSubscription) tuple))
                {
                    tuple.Item2.SetValuesFrom(request);
                }
                else
                {
                    PermanentSubscription newSubscription = new();
                    newSubscription.SetValuesFrom(request);
                    Subscriptions.Add(Context.ConnectionId, (Clients.Client(Context.ConnectionId), newSubscription));
                }
            });
        }

        /// <summary>
        /// Notify the subscribed clients when a change was made in the observable repository.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="eventArgs">The informations about the event</param>
        protected async void Repository_ItemOperationPerformed(object sender, SetItemOperationPerformedEventArgs<Guid> eventArgs)
        {
            switch (eventArgs.OperationType)
            {
                case OperationType.Create:
                    SetItemOperationPerformedNotification createNotification = new(eventArgs.OperationType, eventArgs.ImpactedItemsIds);
                    foreach (KeyValuePair<string, (IClientProxy, PermanentSubscription)> subscription in Subscriptions.Where(pair => pair.Value.Item2.SubscribeItemCreate))
                    {
                        await subscription.Value.Item1.SendAsync(BaseHubEndpoints.NotifySetItemOperationPerformedClientEndpoint, createNotification);
                        if (subscription.Value.Item2.SubscribeCreatedItemUpdate)
                        {
                            subscription.Value.Item2.SubscribedItemsForUpdate.AddRange(eventArgs.ImpactedItemsIds);
                        }
                    }
                    return;

                case OperationType.Delete:
                    SetItemOperationPerformedNotification deleteNotification = new(eventArgs.OperationType, eventArgs.ImpactedItemsIds);
                    foreach (KeyValuePair<string, (IClientProxy, PermanentSubscription)> subscription in Subscriptions.Where(pair => pair.Value.Item2.SubscribeItemDelete))
                    {
                        await subscription.Value.Item1.SendAsync(BaseHubEndpoints.NotifySetItemOperationPerformedClientEndpoint, deleteNotification);
                        subscription.Value.Item2.SubscribedItemsForUpdate.RemoveRange(eventArgs.ImpactedItemsIds);
                    }
                    return;

                case OperationType.Update:
                    foreach (KeyValuePair<string, (IClientProxy, PermanentSubscription)> subscription in Subscriptions)
                    {
                        IEnumerable<Guid> intersect = subscription.Value.Item2.ReadOnlySubscribedItemsForUpdate.Intersect(eventArgs.ImpactedItemsIds);
                        if (intersect.Any())
                        {
                            await subscription.Value.Item1.SendAsync(BaseHubEndpoints.NotifySetItemOperationPerformedClientEndpoint, new SetItemOperationPerformedNotification(eventArgs.OperationType, intersect));
                        }
                    }
                    return;
            }
        }

        /// <inheritdoc cref="Hub.OnDisconnectedAsync(Exception?)"/>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Subscriptions.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}