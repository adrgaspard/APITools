using System;

namespace APIBase.Core.ComponentModel
{
    /// <summary>
    /// Informs when any write operation is done on the object's set.
    /// </summary>
    /// <typeparam name="TItemId">Type of item identifier</typeparam>
    public interface INotifyItemOperationPerformed<TItemId>
    {
        /// <summary>
        /// Occurs when an operation has just been performed.
        /// </summary>
        event EventHandler<SetItemOperationPerformedEventArgs<TItemId>> ItemOperationPerformed;
    }
}