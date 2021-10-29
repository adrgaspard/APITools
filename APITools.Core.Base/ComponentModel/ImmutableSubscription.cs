using System;
using System.Collections.Generic;

namespace APITools.Core.ComponentModel
{
    /// <summary>
    /// Represents a read-only subscription to changes in a set of objects.
    /// </summary>
    public class ImmutableSubscription : ISubscription
    {
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