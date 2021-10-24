using System;

namespace APIBase.Core.Ioc
{
    /// <summary>
    /// Represents a simple Ioc container with basic functionality needed to register and resolve instances.
    /// </summary>
    public interface IServiceManager : IServiceLocator
    {
        /// <summary>
        /// Registers a given type for a given interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface for which instances will be resolved</typeparam>
        /// <typeparam name="TImplementation">The type that must be used to create instances</typeparam>
        void Register<TInterface, TImplementation>() where TInterface : class where TImplementation : class, TInterface;

        /// <summary>
        /// Registers a given type for a given interface with the possibility for immediate creation of the instance.
        /// </summary>
        /// <typeparam name="TInterface">The interface for which instances will be resolved</typeparam>
        /// <typeparam name="TImplementation">The type that must be used to create instances</typeparam>
        /// <param name="createInstanceImmediately">If true, forces the creation of the default instance of the provided class</param>
        void Register<TInterface, TImplementation>(bool createInstanceImmediately) where TInterface : class where TImplementation : class, TInterface;

        /// <summary>
        /// Registers a given type.
        /// </summary>
        /// <typeparam name="TService">The type that must be used to create instances</typeparam>
        void Register<TService>() where TService : class;

        /// <summary>
        /// Registers a given type with the possibility for immediate creation of the instance.
        /// </summary>
        /// <typeparam name="TService">The type that must be used to create instances</typeparam>
        /// <param name="createInstanceImmediately">If true, forces the creation of the default instance of the provided class</param>
        void Register<TService>(bool createInstanceImmediately) where TService : class;

        /// <summary>
        /// Registers a given instance for a given type.
        /// </summary>
        /// <typeparam name="TService">The type that is being registered</typeparam>
        /// <param name="factory">The factory method able to create the instance that must be returned when the given type is resolved</param>
        void Register<TService>(Func<TService> factory) where TService : class;

        /// <summary>
        /// Registers a given instance for a given type with the possibility for immediate creation of the instance.
        /// </summary>
        /// <typeparam name="TService">The type that is being registered</typeparam>
        /// <param name="factory">The factory method able to create the instance that must be returned when the given type is resolved</param>
        /// <param name="createInstanceImmediately">If true, forces the creation of the default instance of the provided class</param>
        void Register<TService>(Func<TService> factory, bool createInstanceImmediately) where TService : class;

        /// <summary>
        /// Registers a given instance for a given type and a given key.
        /// </summary>
        /// <typeparam name="TService">The type that is being registered</typeparam>
        /// <param name="factory">The factory method able to create the instance that must be returned when the given type is resolved</param>
        /// <param name="key">The key for which the given instance is registered</param>
        void Register<TService>(Func<TService> factory, string key) where TService : class;

        /// <summary>
        /// Registers a given instance for a given type and a given key with the possibility for immediate creation of the instance.
        /// </summary>
        /// <typeparam name="TService">The type that is being registered</typeparam>
        /// <param name="factory">The factory method able to create the instance that must be returned when the given type is resolved</param>
        /// <param name="key">The key for which the given instance is registered</param>
        /// <param name="createInstanceImmediately">If true, forces the creation of the default instance of the provided class</param>
        void Register<TService>(Func<TService> factory, string key, bool createInstanceImmediately) where TService : class;

        /// <summary>
        /// Resets the instance in its original states. This deletes all the registrations.
        /// </summary>
        void Reset();

        /// <summary>
        /// Unregisters a class from the cache and removes all the previously created instances.
        /// </summary>
        /// <typeparam name="TService">The class that must be removed</typeparam>
        void Unregister<TService>() where TService : class;

        /// <summary>
        /// Removes the given instance from the cache. The class itself remains registered and can be used to create other instances.
        /// </summary>
        /// <typeparam name="TService">The type of the instance to be removed</typeparam>
        /// <param name="instance">The instance that must be removed</param>
        void Unregister<TService>(TService instance) where TService : class;

        /// <summary>
        /// Removes the instance corresponding to the given key from the cache. The class itself remains registered and can be used to create other instances.
        /// </summary>
        /// <typeparam name="TService">The type of the instance to be removed</typeparam>
        /// <param name="key">The key corresponding to the instance that must be removed</param>
        void Unregister<TService>(string key) where TService : class;
    }
}