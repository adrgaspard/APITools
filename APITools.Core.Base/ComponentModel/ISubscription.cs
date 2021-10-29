using System;
using System.Collections.Generic;

namespace APITools.Core.ComponentModel
{
    /// <summary>
    /// Represent a subscription to changes in a set of objects.
    /// </summary>
    public interface ISubscription
    {
        /// <summary>
        /// Gets a value that indicates wheter the subscription is empty (i.e. it is not affected by any change).
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets the list of all existing items concerned by the subscription.
        /// </summary>
        IReadOnlyCollection<Guid> ReadOnlySubscribedItemsForUpdate { get; }

        /// <summary>
        /// Gets a value that indicates whether the subscription will automatically subscribe to changes in added items.
        /// Does not make sense if SubscribeItemCreate is false.
        /// </summary>
        /// <see cref="SubscribeItemCreate"/>
        bool SubscribeCreatedItemUpdate { get; }

        /// <summary>
        /// Gets a value that indicates whether the subscription is concerned by the addition of new items.
        /// </summary>
        bool SubscribeItemCreate { get; }

        /// <summary>
        /// Gets a value that indicates whether the subscription is concerned by the deletion of existing items.
        /// </summary>
        bool SubscribeItemDelete { get; }
    }
}