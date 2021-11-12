using System;
using System.Collections.Generic;

namespace APITools.Core.Base.Ioc
{
    public interface IServiceLocator : IServiceProvider
    {
        /// <summary>
        /// Checks whether at least one instance of a given class is already created in the container.
        /// </summary>
        /// <typeparam name="TService">The class that is queried</typeparam>
        /// <returns>True if at least on instance of the class is already created, false otherwise</returns>
        bool ContainsCreated<TService>();

        /// <summary>
        /// Checks whether the instance with the given key is already created for a given class in the container.
        /// </summary>
        /// <typeparam name="TService">The class that is queried</typeparam>
        /// <param name="key">The key that is queried</param>
        /// <returns>True if the instance with the given key is already registered for the given class, false otherwise</returns>
        bool ContainsCreated<TService>(string key);

        /// <summary>
        /// Provides a way to get all the created instances of a given type available in the cache. Registering a class or a factory does not automatically create the corresponding instance !
        /// To create an instance, either register the class or the factory with createInstanceImmediately set to true, or call the GetInstance method before calling GetAllCreatedInstances.
        /// Alternatively, use the GetAllInstances method, which auto-creates default instances for all registered classes.
        /// </summary>
        /// <param name="serviceType">The class of which all instances must be returned</param>
        /// <returns>All the already created instances of the given type</returns>
        public IEnumerable<object> GetAllCreatedInstances(Type serviceType);

        /// <summary>
        /// Provides a way to get all the created instances of a given type available in the cache. Registering a class or a factory does not automatically create the corresponding instance !
        /// To create an instance, either register the class or the factory with createInstanceImmediately set to true, or call the GetInstance method before calling GetAllCreatedInstances.
        /// Alternatively, use the GetAllInstances method, which auto-creates default instances for all registered classes.
        /// </summary>
        /// <typeparam name="TService">The class of which all instances must be returned</typeparam>
        /// <returns>All the already created instances of the given type</returns>
        public IEnumerable<TService> GetAllCreatedInstances<TService>();

        /// <summary>
        /// Provides a way to get all the created instances of a given type available in the cache. Calling this method auto-creates default instances for all registered classes.
        /// </summary>
        /// <param name="serviceType">The class of which all instances must be returned</param>
        /// <returns>All the instances of the given type</returns>
        IEnumerable<object> GetAllInstances(Type serviceType);

        /// <summary>
        /// Provides a way to get all the created instances of a given type available in the cache. Calling this method auto-creates default instances for all registered classes.
        /// </summary>
        /// <typeparam name="TService">The class of which all instances must be returned</typeparam>
        /// <returns>All the instances of the given type</returns>
        IEnumerable<TService> GetAllInstances<TService>();

        /// <summary>
        /// Provides a way to get an instance of a given type. If no instance had been instantiated  before, a new instance will be created. If an instance had already been created, that same instance will be returned.
        /// </summary>
        /// <param name="serviceType">The class of which an instance must be returned</param>
        /// <returns>An instance of the given type</returns>
        object GetInstance(Type serviceType);

        /// <summary>
        /// Provides a way to get an instance of a given type corresponding to a given key. If no instance had been instantiated with this key before, a new instance will be created.
        /// If an instance had already been created with the same key, that same instance will be returned.
        /// </summary>
        /// <param name="serviceType">The class of which an instance must be returned</param>
        /// <param name="key">The key uniquely identifying this instance</param>
        /// <returns>An instance corresponding to the given type and key</returns>
        object GetInstance(Type serviceType, string key);

        /// <summary>
        /// Provides a way to get an instance of a given type. If no instance had been instantiated before, a new instance will be created.
        /// If an instance had already been created, that same instance will be returned.
        /// </summary>
        /// <typeparam name="TService">The class of which an instance must be returned</typeparam>
        /// <returns>An instance of the given type</returns>
        TService GetInstance<TService>();

        /// <summary>
        /// Provides a way to get an instance of a given type corresponding to a given key. If no instance had been instantiated with this key before, a new instance will be created.
        /// If an instance had already been created with the same key, that same instance will be returned.
        /// </summary>
        /// <typeparam name="TService">The class of which an instance must be returned</typeparam>
        /// <param name="key">The key uniquely identifying this instance</param>
        /// <returns>An instance corresponding to the given type and key</returns>
        TService GetInstance<TService>(string key);

        /// <summary>
        /// Provides a way to get an instance of a given type. This method always returns a new instance and doesn't cache it in the Ioc container.
        /// </summary>
        /// <param name="serviceType">The class of which an instance must be returned</param>
        /// <returns>An instance of the given type</returns>
        public object GetInstanceWithoutCaching(Type serviceType);

        /// <summary>
        /// Provides a way to get an instance of a given type. This method always returns a new instance and doesn't cache it in the Ioc container.
        /// </summary>
        /// <param name="serviceType">The class of which an instance must be returned</param>
        /// <param name="key">The key uniquely identifying this instance</param>
        /// <returns>An instance corresponding to the given type and key</returns>
        public object GetInstanceWithoutCaching(Type serviceType, string key);

        /// <summary>
        /// Provides a way to get an instance of a given type. This method always returns a new instance and doesn't cache it in the Ioc container.
        /// </summary>
        /// <typeparam name="TService">The class of which an instance must be returned</typeparam>
        /// <returns>An instance of the given type</returns>
        public TService GetInstanceWithoutCaching<TService>();

        /// <summary>
        /// Provides a way to get an instance of a given type. This method always returns a new instance and doesn't cache it in the Ioc container.
        /// </summary>
        /// <typeparam name="TService">The class of which an instance must be returned</typeparam>
        /// <param name="key">The key uniquely identifying this instance</param>
        /// <returns>An instance corresponding to the given type and key</returns>
        public TService GetInstanceWithoutCaching<TService>(string key);

        /// <summary>
        /// Gets a value indicating whether a given type T is already registered.
        /// </summary>
        /// <typeparam name="TService">The type that the method checks for</typeparam>
        /// <returns>True if the type is registered, false otherwise</returns>
        bool IsRegistered<TService>();

        /// <summary>
        /// Gets a value indicating whether a given type T and a give key are already registered.
        /// </summary>
        /// <typeparam name="TService">The type that the method checks for</typeparam>
        /// <param name="key">The key that the method checks for</param>
        /// <returns>True if the type and key are registered, false otherwise</returns>
        bool IsRegistered<TService>(string key);
    }
}