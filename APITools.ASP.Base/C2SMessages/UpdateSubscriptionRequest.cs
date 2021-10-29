using APITools.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace APITools.ASP.Requests.C2SMessages
{
    /// <summary>
    /// Represents a subscription update request to the events of a restful-like hub.
    /// </summary>
    public class UpdateSubscriptionRequest : ISubscription
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public UpdateSubscriptionRequest() { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="model">The subscription to be copied</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="model"/> is null</exception>
        public UpdateSubscriptionRequest(ISubscription model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            SubscribeCreatedItemUpdate = model.SubscribeCreatedItemUpdate;
            ReadOnlySubscribedItemsForUpdate = new ReadOnlyCollection<Guid>(new List<Guid>(model.ReadOnlySubscribedItemsForUpdate));
            SubscribeItemCreate = model.SubscribeItemCreate;
            SubscribeItemDelete = model.SubscribeItemDelete;
        }

        /// <summary>
        /// Gets a value that indicates wheter the subscription is empty (i.e. it is not affected by any change).
        /// </summary>
        public bool IsEmpty => !SubscribeItemCreate && !SubscribeItemDelete && (ReadOnlySubscribedItemsForUpdate is null || ReadOnlySubscribedItemsForUpdate.Count == 0);

        /// <summary>
        /// Gets or sets the list of all existing items concerned by the subscription.
        /// </summary>
        public IReadOnlyCollection<Guid> ReadOnlySubscribedItemsForUpdate { get; init; }

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription will automatically subscribe to changes in added items.
        /// Does not make sense if SubscribeItemCreate is false.
        /// </summary>
        /// <see cref="SubscribeItemCreate"/>
        public bool SubscribeCreatedItemUpdate { get; init; }

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription is concerned by the addition of new items.
        /// </summary>
        public bool SubscribeItemCreate { get; init; }

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription is concerned by the deletion of existing items.
        /// </summary>
        public bool SubscribeItemDelete { get; init; }
    }
}