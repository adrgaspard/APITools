using System;

namespace APITools.Core.Ioc
{
    /// <summary>
    /// Represents the entry point for the service locator.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// Create the unique mutex.
        /// </summary>
        static ServiceLocator()
        {
            InstanceLock = new();
        }

        /// <summary>
        /// Gets or sets the main service locator.
        /// </summary>
        public static IServiceLocator Current { get; private set; }

        /// <summary>
        /// Gets or sets the unique mutex.
        /// </summary>
        private static object InstanceLock { get; set; }

        /// <summary>
        /// Sets the new main service locator.
        /// </summary>
        /// <param name="serviceLocator"></param>
        /// <exception cref="InvalidOperationException">Occurs when the service locator is already defined</exception>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="serviceLocator"/> is null</exception>
        public static void SetCurrentServiceLocator(IServiceLocator serviceLocator)
        {
            lock (InstanceLock)
            {
                if (Current is not null)
                {
                    throw new InvalidOperationException("The current service locator can't be redefined");
                }
                if (serviceLocator is null)
                {
                    throw new ArgumentNullException(nameof(serviceLocator));
                }
                Current = serviceLocator;
            }
        }
    }
}