using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;

namespace APIBase.Core.MVVM
{
    /// <summary>
    /// Represents the base class for all the viewmodels classes.
    /// </summary>
    public abstract class ViewModel : ObservableObject, IDisposable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected ViewModel() : this(WeakReferenceMessenger.Default)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="messenger">The Messenger instance to be used by the viewmodel</param>
        protected ViewModel(IMessenger messenger)
        {
            Messenger = messenger;
        }

        /// <summary>
        /// Gets or sets the messenger instance to be used by the viewmodel.
        /// </summary>
        protected IMessenger Messenger { get; set; }

        /// <summary>
        ///
        /// </summary>
        public virtual void Dispose()
        {
            Messenger.UnregisterAll(this);
            GC.SuppressFinalize(this);
        }
    }
}