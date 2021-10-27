using APIBase.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace APIBase.Core.ComponentModel
{
    /// <summary>
    /// Represents a permanent subscription to changes in a set of objects.
    /// It can evolve over time.
    /// </summary>
    public class PermanentSubscription : ISubscription, INotifyPropertyChanged
    {
        /// <summary>
        /// Creates a new instance.
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
                SubscribedItemsForUpdate = new List<Guid>();
            }
            else
            {
                SubscribedItemsForUpdate = new List<Guid>(items);
            }
            ReadOnlySubscribedItemsForUpdate = new ReadOnlyCollection<Guid>(SubscribedItemsForUpdate);
        }

        /// <summary>
        /// Gets a value that indicates wheter the subscription is empty (i.e. it is not affected by any change).
        /// </summary>
        public bool IsEmpty => !SubscribeItemCreate && !SubscribeItemDelete && (ReadOnlySubscribedItemsForUpdate is null || ReadOnlySubscribedItemsForUpdate.Count == 0);

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription will automatically subscribe to changes in added items.
        /// Does not make sense if SubscribeItemCreate is false.
        /// </summary>
        /// <see cref="SubscribeItemCreate"/>
        public bool SubscribeCreatedItemUpdate { get; private set; }

        public IList<Guid> SubscribedItemsForUpdate { get; private set; }

        /// <summary>
        /// Gets or sets the list of all existing items identifiers concerned by the subscription.
        /// </summary>
        public IReadOnlyCollection<Guid> ReadOnlySubscribedItemsForUpdate { get; private set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription is concerned by the addition of new items.
        /// </summary>
        public bool SubscribeItemCreate { get; private set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the subscription is concerned by the deletion of existing items.
        /// </summary>
        public bool SubscribeItemDelete { get; private set; }

        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Copies and sets values from another subscription.
        /// </summary>
        /// <param name="subscription">The subscription to be copied</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="subscription"/> is null</exception>
        public void SetValuesFrom(ISubscription subscription)
        {
            if (subscription is null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }
            SubscribeItemCreate = subscription.SubscribeItemCreate;
            SubscribeCreatedItemUpdate = subscription.SubscribeCreatedItemUpdate;
            SubscribeItemDelete = subscription.SubscribeItemDelete;
            SubscribedItemsForUpdate.Clear();
            SubscribedItemsForUpdate.AddRange(subscription.ReadOnlySubscribedItemsForUpdate);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}