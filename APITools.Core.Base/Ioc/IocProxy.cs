using System;
using System.Collections.Generic;

namespace APITools.Core.Base.Ioc
{
    /// <summary>
    /// Represents a read-only Ioc container with basic functionality needed to resolve instances.
    /// </summary>
    public class IocProxy : IServiceLocator
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="baseServiceLocator">The base service locator to use</param>
        public IocProxy(IServiceLocator baseServiceLocator)
        {
            BaseServiceLocator = baseServiceLocator;
        }

        /// <summary>
        /// Gets or sets the base service locator used by the proxy.
        /// </summary>
        protected IServiceLocator BaseServiceLocator { get; init; }

        /// <inheritdoc cref="IServiceLocator.ContainsCreated{TService}"/>
        public bool ContainsCreated<TService>()
        {
            return BaseServiceLocator.ContainsCreated<TService>();
        }

        /// <inheritdoc cref="IServiceLocator.ContainsCreated{TService}(string)"/>
        public bool ContainsCreated<TService>(string key)
        {
            return BaseServiceLocator.ContainsCreated<TService>(key);
        }

        /// <inheritdoc cref="IServiceLocator.GetAllCreatedInstances(Type)"/>
        public IEnumerable<object> GetAllCreatedInstances(Type serviceType)
        {
            return BaseServiceLocator.GetAllCreatedInstances(serviceType);
        }

        /// <inheritdoc cref="IServiceLocator.GetAllCreatedInstances{TService}"/>
        public IEnumerable<TService> GetAllCreatedInstances<TService>()
        {
            return BaseServiceLocator.GetAllCreatedInstances<TService>();
        }

        /// <inheritdoc cref="IServiceLocator.GetAllInstances(Type)"/>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return BaseServiceLocator.GetAllInstances(serviceType);
        }

        /// <inheritdoc cref="IServiceLocator.GetAllInstances{TService}"/>
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return BaseServiceLocator.GetAllInstances<TService>();
        }

        /// <inheritdoc cref="IServiceLocator.GetInstance(Type)"/>
        public object GetInstance(Type serviceType)
        {
            return BaseServiceLocator.GetInstance(serviceType);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstance(Type, string)"/>
        public object GetInstance(Type serviceType, string key)
        {
            return BaseServiceLocator.GetInstance(serviceType, key);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstance{TService}"/>
        public TService GetInstance<TService>()
        {
            return BaseServiceLocator.GetInstance<TService>();
        }

        /// <inheritdoc cref="IServiceLocator.GetInstance{TService}(string)"/>
        public TService GetInstance<TService>(string key)
        {
            return BaseServiceLocator.GetInstance<TService>(key);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstanceWithoutCaching(Type)"/>
        public object GetInstanceWithoutCaching(Type serviceType)
        {
            return BaseServiceLocator.GetInstanceWithoutCaching(serviceType);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstanceWithoutCaching(Type, string)"/>
        public object GetInstanceWithoutCaching(Type serviceType, string key)
        {
            return BaseServiceLocator.GetInstanceWithoutCaching(serviceType, key);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstanceWithoutCaching{TService}"/>
        public TService GetInstanceWithoutCaching<TService>()
        {
            return BaseServiceLocator.GetInstanceWithoutCaching<TService>();
        }

        /// <inheritdoc cref="IServiceLocator.GetInstanceWithoutCaching{TService}(string)"/>
        public TService GetInstanceWithoutCaching<TService>(string key)
        {
            return BaseServiceLocator.GetInstanceWithoutCaching<TService>(key);
        }

        /// <inheritdoc cref="IServiceProvider.GetService(Type)"/>
        public object GetService(Type serviceType)
        {
            return BaseServiceLocator.GetService(serviceType);
        }

        /// <inheritdoc cref="IServiceLocator.IsRegistered{TService}"/>
        public bool IsRegistered<TService>()
        {
            return BaseServiceLocator.IsRegistered<TService>();
        }

        /// <inheritdoc cref="IServiceLocator.IsRegistered{TService}(string)"/>
        public bool IsRegistered<TService>(string key)
        {
            return BaseServiceLocator.IsRegistered<TService>(key);
        }
    }
}