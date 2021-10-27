using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace APIBase.Core.Ioc
{
    /// <summary>
    /// A Ioc container with basic functionality needed to register and resolve instances.
    /// </summary>
    public class IocContainer : IServiceManager
    {
        /// <summary>
        /// The constructor method name.
        /// </summary>
        protected const string ConstructorName = ".cctor";

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public IocContainer()
        {
            ConstructorInfos = new Dictionary<Type, ConstructorInfo>();
            DefaultKey = Guid.NewGuid().ToString();
            EmptyArguments = Array.Empty<object>();
            Factories = new Dictionary<Type, IDictionary<string, Delegate>>();
            InstancesRegistry = new Dictionary<Type, IDictionary<string, object>>();
            InterfacesToImplementationsMap = new Dictionary<Type, Type>();
            SyncLock = new();
        }

        /// <summary>
        /// Gets or sets the constructor infos for all registered types.
        /// </summary>
        protected IDictionary<Type, ConstructorInfo> ConstructorInfos { get; init; }

        /// <summary>
        /// Gets or sets the default key of the container.
        /// </summary>
        protected string DefaultKey { get; init; }

        /// <summary>
        /// Gets or sets an array that represents empty arguments.
        /// </summary>
        protected object[] EmptyArguments { get; init; }

        /// <summary>
        /// Gets or sets the factories used by the container.
        /// </summary>
        protected IDictionary<Type, IDictionary<string, Delegate>> Factories { get; init; }

        /// <summary>
        /// Gets or sets the instances managed by the container.
        /// </summary>
        protected IDictionary<Type, IDictionary<string, object>> InstancesRegistry { get; init; }

        /// <summary>
        /// Gets or sets the interface/implementation map.
        /// </summary>
        protected IDictionary<Type, Type> InterfacesToImplementationsMap { get; init; }

        /// <summary>
        /// Gets or sets the synchronization mutex.
        /// </summary>
        protected object SyncLock { get; init; }

        /// <inheritdoc cref="IServiceLocator.ContainsCreated{TService}"/>
        public bool ContainsCreated<TService>()
        {
            return ContainsCreated<TService>(null);
        }

        /// <inheritdoc cref="IServiceLocator.ContainsCreated{TService}(string)"/>
        public bool ContainsCreated<TService>(string key)
        {
            Type serviceType = typeof(TService);
            if (!InstancesRegistry.ContainsKey(serviceType))
            {
                return false;
            }
            if (string.IsNullOrEmpty(key))
            {
                return InstancesRegistry[serviceType].Count > 0;
            }
            return InstancesRegistry[serviceType].ContainsKey(key);
        }

        /// <inheritdoc cref="IServiceLocator.GetAllCreatedInstances(Type)"/>
        public IEnumerable<object> GetAllCreatedInstances(Type serviceType)
        {
            if (InstancesRegistry.ContainsKey(serviceType))
            {
                return InstancesRegistry[serviceType].Values;
            }
            return new List<object>();
        }

        /// <inheritdoc cref="IServiceLocator.GetAllCreatedInstances{TService}"/>
        public IEnumerable<TService> GetAllCreatedInstances<TService>()
        {
            return GetAllCreatedInstances(typeof(TService)).Cast<TService>();
        }

        /// <inheritdoc cref="IServiceLocator.GetAllInstances(Type)"/>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            lock (Factories)
            {
                if (Factories.ContainsKey(serviceType))
                {
                    foreach (KeyValuePair<string, Delegate> factory in Factories[serviceType])
                    {
                        GetInstance(serviceType, factory.Key);
                    }
                }
            }
            if (InstancesRegistry.ContainsKey(serviceType))
            {
                return InstancesRegistry[serviceType].Values;
            }
            return new List<object>();
        }

        /// <inheritdoc cref="IServiceLocator.GetAllInstances{TService}"/>
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return GetAllInstances(typeof(TService)).Cast<TService>();
        }

        /// <inheritdoc cref="IServiceLocator.GetInstance(Type)"/>
        public object GetInstance(Type serviceType)
        {
            return DoGetService(serviceType, DefaultKey);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstance(Type, string)"/>
        public object GetInstance(Type serviceType, string key)
        {
            return DoGetService(serviceType, key);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstance{TService}"/>
        public TService GetInstance<TService>()
        {
            return (TService)DoGetService(typeof(TService), DefaultKey);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstance{TService}(string)"/>
        public TService GetInstance<TService>(string key)
        {
            return (TService)DoGetService(typeof(TService), key);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstanceWithoutCaching(Type)"/>
        public object GetInstanceWithoutCaching(Type serviceType)
        {
            return DoGetService(serviceType, DefaultKey, false);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstanceWithoutCaching(Type, string)"/>
        public object GetInstanceWithoutCaching(Type serviceType, string key)
        {
            return DoGetService(serviceType, key, false);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstanceWithoutCaching{TService}"/>
        public TService GetInstanceWithoutCaching<TService>()
        {
            return (TService)DoGetService(typeof(TService), DefaultKey, false);
        }

        /// <inheritdoc cref="IServiceLocator.GetInstanceWithoutCaching{TService}(string)"/>
        public TService GetInstanceWithoutCaching<TService>(string key)
        {
            return (TService)DoGetService(typeof(TService), key, false);
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <returns>A service object of type <paramref name="serviceType"/></returns>
        /// <param name="serviceType">An object that specifies the type of service object to get</param>
        public object GetService(Type serviceType)
        {
            return DoGetService(serviceType, DefaultKey);
        }

        /// <inheritdoc cref="IServiceLocator.IsRegistered{TService}"/>
        public bool IsRegistered<TService>()
        {
            return InterfacesToImplementationsMap.ContainsKey(typeof(TService));
        }

        /// <inheritdoc cref="IServiceLocator.IsRegistered{TService}(string)"/>
        public bool IsRegistered<TService>(string key)
        {
            Type serviceType = typeof(TService);
            if (!InterfacesToImplementationsMap.ContainsKey(serviceType) || !Factories.ContainsKey(serviceType))
            {
                return false;
            }
            return Factories[serviceType].ContainsKey(key);
        }

        /// <inheritdoc cref="IServiceManager.Register{TInterface, TImplementation}"/>
        public void Register<TInterface, TImplementation>() where TInterface : class where TImplementation : class, TInterface
        {
            Register<TInterface, TImplementation>(false);
        }

        /// <inheritdoc cref="IServiceManager.Register{TInterface, TImplementation}(bool)"/>
        /// <exception cref="InvalidOperationException">Occurs when there is already a class registered for <typeparamref name="TInterface"/></exception>
        public void Register<TInterface, TImplementation>(bool createInstanceImmediately) where TInterface : class where TImplementation : class, TInterface
        {
            lock (SyncLock)
            {
                Type interfaceType = typeof(TInterface);
                Type classType = typeof(TImplementation);
                if (InterfacesToImplementationsMap.ContainsKey(interfaceType))
                {
                    if (InterfacesToImplementationsMap[interfaceType] != classType)
                    {
                        throw new InvalidOperationException($"There is already a class registered for {interfaceType.FullName}");
                    }
                }
                else
                {
                    InterfacesToImplementationsMap.Add(interfaceType, classType);
                    ConstructorInfos.Add(classType, GetConstructorInfo(classType));
                }
                Func<TInterface> factory = MakeInstance<TInterface>;
                DoRegister(interfaceType, factory, DefaultKey);
                if (createInstanceImmediately)
                {
                    GetInstance<TInterface>();
                }
            }
        }

        /// <inheritdoc cref="IServiceManager.Register{TService}"/>
        public void Register<TService>() where TService : class
        {
            Register<TService>(false);
        }

        /// <inheritdoc cref="IServiceManager.Register{TService}(bool)"/>
        /// <exception cref="ArgumentException">Occurs if the type <typeparamref name="TService"/> is an interface type</exception>
        /// <exception cref="InvalidOperationException">Occurs when the type <typeparamref name="TService"/> is already registered</exception>
        public void Register<TService>(bool createInstanceImmediately) where TService : class
        {
            Type serviceType = typeof(TService);
            if (serviceType.IsInterface)
            {
                throw new ArgumentException("An interface cannot be registered alone");
            }
            lock (SyncLock)
            {
                if (Factories.ContainsKey(serviceType) && Factories[serviceType].ContainsKey(DefaultKey))
                {
                    if (!ConstructorInfos.ContainsKey(serviceType))
                    {
                        throw new InvalidOperationException($"Class {serviceType} is already registered.");
                    }
                    return;
                }
                if (!InterfacesToImplementationsMap.ContainsKey(serviceType))
                {
                    InterfacesToImplementationsMap.Add(serviceType, null);
                }
                ConstructorInfos.Add(serviceType, GetConstructorInfo(serviceType));
                Func<TService> factory = MakeInstance<TService>;
                DoRegister(serviceType, factory, DefaultKey);
                if (createInstanceImmediately)
                {
                    GetInstance<TService>();
                }
            }
        }

        /// <inheritdoc cref="IServiceManager.Register{TService}(Func{TService})"/>
        public void Register<TService>(Func<TService> factory) where TService : class
        {
            Register(factory, false);
        }

        /// <inheritdoc cref="IServiceManager.Register{TService}(Func{TService}, bool)"/>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="factory"/> is null</exception>
        public void Register<TService>(Func<TService> factory, bool createInstanceImmediately) where TService : class
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            lock (SyncLock)
            {
                Type serviceType = typeof(TService);
                if (Factories.ContainsKey(serviceType) && Factories[serviceType].ContainsKey(DefaultKey))
                {
                    throw new InvalidOperationException($"There is already a factory registered for {serviceType.FullName}");
                }
                if (!InterfacesToImplementationsMap.ContainsKey(serviceType))
                {
                    InterfacesToImplementationsMap.Add(serviceType, null);
                }
                DoRegister(serviceType, factory, DefaultKey);
                if (createInstanceImmediately)
                {
                    GetInstance<TService>();
                }
            }
        }

        /// <inheritdoc cref="IServiceManager.Register{TService}(Func{TService}, string)"/>
        public void Register<TService>(Func<TService> factory, string key) where TService : class
        {
            Register(factory, key, false);
        }

        /// <inheritdoc cref="IServiceManager.Register{TService}(Func{TService}, string, bool)"/>
        /// <exception cref="ArgumentNullException">Occurs when the <paramref name="factory"/> is null</exception>
        /// <exception cref="InvalidOperationException">Occurs when there is already a factory registered for the type <typeparamref name="TService"/> with the key <paramref name="key"/></exception>
        public void Register<TService>(Func<TService> factory, string key, bool createInstanceImmediately) where TService : class
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            lock (SyncLock)
            {
                Type serviceType = typeof(TService);
                if (Factories.ContainsKey(serviceType) && Factories[serviceType].ContainsKey(key))
                {
                    throw new InvalidOperationException($"There is already a factory registered for {serviceType.FullName} with key {key}");
                }
                if (!InterfacesToImplementationsMap.ContainsKey(serviceType))
                {
                    InterfacesToImplementationsMap.Add(serviceType, null);
                }
                DoRegister(serviceType, factory, key);
                if (createInstanceImmediately)
                {
                    GetInstance<TService>(key);
                }
            }
        }

        /// <inheritdoc cref="IServiceManager.Reset"/>
        public void Reset()
        {
            InterfacesToImplementationsMap.Clear();
            InstancesRegistry.Clear();
            ConstructorInfos.Clear();
            Factories.Clear();
        }

        /// <inheritdoc cref="IServiceManager.Unregister{TService}"/>
        public void Unregister<TService>() where TService : class
        {
            lock (SyncLock)
            {
                Type serviceType = typeof(TService);
                Type resolveTo;
                if (InterfacesToImplementationsMap.ContainsKey(serviceType))
                {
                    resolveTo = InterfacesToImplementationsMap[serviceType] ?? serviceType;
                }
                else
                {
                    resolveTo = serviceType;
                }
                if (InstancesRegistry.ContainsKey(serviceType))
                {
                    InstancesRegistry.Remove(serviceType);
                }
                if (InterfacesToImplementationsMap.ContainsKey(serviceType))
                {
                    InterfacesToImplementationsMap.Remove(serviceType);
                }
                if (Factories.ContainsKey(serviceType))
                {
                    Factories.Remove(serviceType);
                }
                if (ConstructorInfos.ContainsKey(resolveTo))
                {
                    ConstructorInfos.Remove(resolveTo);
                }
            }
        }

        /// <inheritdoc cref="IServiceManager.Unregister{TService}(TService)"/>
        public void Unregister<TService>(TService instance) where TService : class
        {
            lock (SyncLock)
            {
                Type serviceType = typeof(TService);
                if (InstancesRegistry.ContainsKey(serviceType))
                {
                    IDictionary<string, object> list = InstancesRegistry[serviceType];
                    List<KeyValuePair<string, object>> pairs = list.Where(pair => pair.Value == instance).ToList();
                    for (int index = 0; index < pairs.Count; index++)
                    {
                        string key = pairs[index].Key;
                        list.Remove(key);
                    }
                }
            }
        }

        /// <inheritdoc cref="IServiceManager.Unregister{TService}(string)"/>
        public void Unregister<TService>(string key) where TService : class
        {
            lock (SyncLock)
            {
                Type serviceType = typeof(TService);
                if (InstancesRegistry.ContainsKey(serviceType))
                {
                    IDictionary<string, object> list = InstancesRegistry[serviceType];
                    List<KeyValuePair<string, object>> pairs = list.Where(pair => pair.Key == key).ToList();
                    for (int index = 0; index < pairs.Count; index++)
                    {
                        list.Remove(pairs[index].Key);
                    }
                }
                if (Factories.ContainsKey(serviceType))
                {
                    if (Factories[serviceType].ContainsKey(key))
                    {
                        Factories[serviceType].Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the main constructor for a given type.
        /// </summary>
        /// <param name="constructorInfos">The constructors of the type</param>
        /// <param name="resolveTo">The type to be checked</param>
        /// <returns>The main constructor</returns>
        /// <exception cref="InvalidOperationException">Occurs when the type has multiple constructors but none marked with the MainConstructor attribute</exception>
        protected static ConstructorInfo GetMainConstructorInfo(IEnumerable<ConstructorInfo> constructorInfos, Type resolveTo)
        {
            Type mainConstructorAttributeType = typeof(MainConstructorAttribute);
            ConstructorInfo mainConstructorInfo = (from constructor in constructorInfos
                                                   let attribute = Attribute.GetCustomAttribute(constructor, mainConstructorAttributeType)
                                                   where attribute is not null
                                                   select constructor).FirstOrDefault();
            if (mainConstructorInfo is null)
            {
                throw new InvalidOperationException($"Multiple constructors found in {resolveTo.Name} but none marked with {mainConstructorAttributeType.Name}");
            }
            return mainConstructorInfo;
        }

        /// <summary>
        /// Gets a service based on several parameters.
        /// </summary>
        /// <param name="serviceType">The wanted type (of the service)</param>
        /// <param name="key">The key of the service</param>
        /// <param name="cache">A value that indicates whether the service will be searched in the cache or not (if not, a new instance will be created each time the method is called)</param>
        /// <returns>The wanted service</returns>
        /// <exception cref="InvalidOperationException">Occures when <paramref name="serviceType"/> is not found in cache without a key or the desired key</exception>
        protected object DoGetService(Type serviceType, string key, bool cache = true)
        {
            lock (SyncLock)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = DefaultKey;
                }
                IDictionary<string, object> instances = null;
                if (!InstancesRegistry.ContainsKey(serviceType))
                {
                    if (!InterfacesToImplementationsMap.ContainsKey(serviceType))
                    {
                        throw new InvalidOperationException($"The type {serviceType.FullName} was not found in cache");
                    }
                    if (cache)
                    {
                        instances = new Dictionary<string, object>();
                        InstancesRegistry.Add(serviceType, instances);
                    }
                }
                else
                {
                    instances = InstancesRegistry[serviceType];
                }
                if (instances is not null && instances.ContainsKey(key))
                {
                    return instances[key];
                }
                object instance = null;
                if (Factories.ContainsKey(serviceType))
                {
                    if (Factories[serviceType].ContainsKey(key))
                    {
                        instance = Factories[serviceType][key].DynamicInvoke(null);
                    }
                    else
                    {
                        if (Factories[serviceType].ContainsKey(DefaultKey))
                        {
                            instance = Factories[serviceType][DefaultKey].DynamicInvoke(null);
                        }
                        else
                        {
                            throw new InvalidOperationException($"The type {serviceType.FullName} was not found without a key");
                        }
                    }
                }
                if (cache && instances is not null)
                {
                    instances.Add(key, instance);
                }
                return instance;
            }
        }

        /// <summary>
        /// Register a factory for a service.
        /// </summary>
        /// <typeparam name="TService">The result type of the service factory</typeparam>
        /// <param name="serviceType">The type of the service (will serve as a type key)</param>
        /// <param name="factory">The factory to register</param>
        /// <param name="key">The instance key of the factory</param>
        protected void DoRegister<TService>(Type serviceType, Func<TService> factory, string key)
        {
            if (Factories.ContainsKey(serviceType))
            {
                if (Factories[serviceType].ContainsKey(key))
                {
                    return;
                }
                Factories[serviceType].Add(key, factory);
            }
            else
            {
                Dictionary<string, Delegate> list = new() { { key, factory } };
                Factories.Add(serviceType, list);
            }
        }

        /// <summary>
        /// Gets the constructor to use for a type.
        /// </summary>
        /// <param name="serviceType">The concerned type</param>
        /// <returns>The constructor to use for <paramref name="serviceType"/></returns>
        /// <exception cref="InvalidOperationException">Occurs when <paramref name="serviceType"/> has no public constructor</exception>
        protected ConstructorInfo GetConstructorInfo(Type serviceType)
        {
            Type resolveTo;
            if (InterfacesToImplementationsMap.ContainsKey(serviceType))
            {
                resolveTo = InterfacesToImplementationsMap[serviceType] ?? serviceType;
            }
            else
            {
                resolveTo = serviceType;
            }
            ConstructorInfo[] constructorInfos = resolveTo.GetConstructors();
            if (constructorInfos.Length > 1)
            {
                if (constructorInfos.Length > 2)
                {
                    return GetMainConstructorInfo(constructorInfos, resolveTo);
                }
                if (constructorInfos.FirstOrDefault(constructor => constructor.Name == ConstructorName) is null)
                {
                    return GetMainConstructorInfo(constructorInfos, resolveTo);
                }
                ConstructorInfo first = constructorInfos.FirstOrDefault(constructor => constructor.Name != ConstructorName);
                if (first is null || !first.IsPublic)
                {
                    throw new InvalidOperationException($"The type {resolveTo.Name} has no public constructor");
                }
                return first;
            }
            if (constructorInfos.Length == 0 || (constructorInfos.Length == 1 && !constructorInfos[0].IsPublic))
            {
                throw new InvalidOperationException($"The type {resolveTo.Name} has no public constructor");
            }
            return constructorInfos[0];
        }

        /// <summary>
        /// Creates an instance of a service.
        /// </summary>
        /// <typeparam name="TService">The type of the service</typeparam>
        /// <returns>The new instance of the service</returns>
        protected TService MakeInstance<TService>()
        {
            Type serviceType = typeof(TService);
            ConstructorInfo constructor = ConstructorInfos.ContainsKey(serviceType) ? ConstructorInfos[serviceType] : GetConstructorInfo(serviceType);
            ParameterInfo[] parameterInfos = constructor.GetParameters();
            if (parameterInfos.Length == 0)
            {
                return (TService)constructor.Invoke(EmptyArguments);
            }
            object[] parameters = new object[parameterInfos.Length];
            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                parameters[parameterInfo.Position] = GetService(parameterInfo.ParameterType);
            }
            return (TService)constructor.Invoke(parameters);
        }
    }
}