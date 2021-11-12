using APITools.Core.Base.DAO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace APITools.ASP.Base.S2CMessages
{
    /// <summary>
    /// Represents an operation performed in a set of items
    /// </summary>
    public class SetItemOperationPerformedNotification
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="operationType">Type of the operation</param>
        /// <param name="impactedItemsIds">List of identifiers of all items impacted by the operation</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="impactedItemsIds"/> is null</exception>
        public SetItemOperationPerformedNotification(OperationType operationType, IEnumerable<Guid> impactedItemsIds)
        {
            OperationType = operationType;
            if (impactedItemsIds is null)
            {
                throw new ArgumentNullException(nameof(impactedItemsIds));
            }
            ImpactedItemsIds = new ReadOnlyCollection<Guid>(new List<Guid>(impactedItemsIds));
        }

        /// <summary>
        /// Gets or sets the list of identifiers of all items impacted by the operation.
        /// </summary>
        public IReadOnlyCollection<Guid> ImpactedItemsIds { get; init; }

        /// <summary>
        /// Gets or sets the type of the operation.
        /// </summary>
        public OperationType OperationType { get; init; }
    }
}