using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace APIBase.Core.ComponentModel
{
    /// <summary>
    /// Represents a permanent subscription to changes in a set of objects.
    /// It can evolve over time.
    /// </summary>
    public class PermanentSubscription : ISubscription
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="subscribeItemCreate">Whether the subscription is concerned by the addition of new items</param>
        /// <param name="subscribeCreatedItemUpdate">Whether the subscription will automatically subscribe to changes in added items (does not make sense if SubscribeItemCreate is false)</param>
        /// <param name="subscribeItemDelete">Whether the subscription is concerned by the deletion of existing items</param>
        /// <param name="items">List of all existing items identifiers concerned by the subscription</param>
        public PermanentSubscription(bool subscribeItemCreate = false, bool subscribeCreatedItemUpdate = false, bool subscribeItemDelete = false, IEnumerable<Guid> items = null)
        {
            SubscribeItemCreate = subscribeItemCreate;
            SubscribeCreatedItemUpdate = subscribeCreatedItemUpdate;
            SubscribeItemDelete = subscribeItemDelete;
            if (items is null)
            {
                SubscribedItemsForUpdate = new ReadOnlyCollection<Guid>(new List<Guid>());
            }
            else
            {
                SubscribedItemsForUpdate = new ReadOnlyCollection<Guid>(new List<Guid>(items));
            }
        }

        /// <summary>
        /// Gets a value that indicates wheter the subscription is empty (i.e. it is not affected by any change).
        /// </summary>
        public bool IsEmpty => !SubscribeItemCreate && !SubscribeItemDelete && (SubscribedItemsForUpdate is null || SubscribedItemsForUpdate.Count == 0);

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription will automatically subscribe to changes in added items.
        /// Does not make sense if SubscribeItemCreate is false.
        /// </summary>
        /// <see cref="SubscribeItemCreate"/>
        public bool SubscribeCreatedItemUpdate { get; protected set; }

        /// <summary>
        /// Gets or sets the list of all existing items identifiers concerned by the subscription.
        /// </summary>
        public IReadOnlyCollection<Guid> SubscribedItemsForUpdate { get; protected set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription is concerned by the addition of new items.
        /// </summary>
        public bool SubscribeItemCreate { get; protected set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription is concerned by the deletion of existing items.
        /// </summary>
        public bool SubscribeItemDelete { get; protected set; }

        /// <summary>
        /// Copies and sets values from another subscription.
        /// </summary>
        /// <param name="subscription">The subscription to be copied</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="subscription"/> is null</exception>
        public void CopyValues(ISubscription subscription)
        {
            if (subscription is null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }
            SubscribeItemCreate = subscription.SubscribeItemCreate;
            SubscribeCreatedItemUpdate = subscription.SubscribeCreatedItemUpdate;
            SubscribeItemDelete = subscription.SubscribeItemDelete;
            SubscribedItemsForUpdate = new ReadOnlyCollection<Guid>(new List<Guid>(subscription.SubscribedItemsForUpdate));
        }
    }
}