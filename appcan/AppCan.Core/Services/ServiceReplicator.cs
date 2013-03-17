/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

using Microsoft.Practices.Prism.Events;

namespace AppCan.Core.Services
{
    /// <summary>
    /// Internal class for keeping service information
    /// </summary>
    internal class ServiceInfo
    {
        public Type FromType;
        public Type ToType;
        public object Instance;

        public ServiceInfo()
        {

        }

        public ServiceInfo(Type fromType, Type toType, object instance)
        {
            FromType = fromType;
            ToType = toType;
            Instance = instance;

        }
    }

    /// <summary>
    /// This service can proxy RegisterType and RegisterInstance calls to the IUnityContainer and later be used to replicate the instances to 
    /// any new contexts that the types should be replicated to.
    /// </summary>
    public class ServiceReplicator : AppCan.Core.Services.IServiceReplicator
    {
        private IUnityContainer _container;
        private List<ServiceInfo> _services = new List<ServiceInfo>();


        /// <summary>
        /// Constructor that takes the root container
        /// </summary>
        /// <param name="container">root container</param>
        public ServiceReplicator(IUnityContainer container)
        {
            _container = container;

            
        }

        /// <summary>
        /// Constructor that takes the root container and allows registration of default services that will be replicated
        /// </summary>
        /// <param name="container">The root container</param>
        /// <param name="registerDefaultServices">true to register the default services, false otherwise</param>
        public ServiceReplicator(IUnityContainer container,bool registerDefaultServices) : this(container)
        {
            if (registerDefaultServices)
            {
                IEventAggregator aggregator = container.Resolve<IEventAggregator>();
                ReplicateInstance<IEventAggregator>(aggregator);

            }


        }

        /// <summary>
        /// Register the type with the container and keep the registration in the list for replication
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IUnityContainer RegisterType<T>(LifetimeManager lifetimeManager=null)
        {
            
            IUnityContainer container;
            if (lifetimeManager==null)
                container=_container.RegisterType<T>();
            else
                container = _container.RegisterType<T>(lifetimeManager);

            _services.Add(new ServiceInfo(typeof(T), null, null));

            return container;
        }

        /// <summary>
        /// Register the type of the container and keep the registration in the list for replication
        /// </summary>
        /// <typeparam name="FromType"></typeparam>
        /// <typeparam name="ToType"></typeparam>
        /// <returns></returns>
        public IUnityContainer RegisterType<FromType,ToType>(LifetimeManager lifetimeManager=null)
        {
            
            IUnityContainer container;
            if (lifetimeManager == null)
                container = _container.RegisterType(typeof(FromType), typeof(ToType));
            else
                container = _container.RegisterType(typeof(FromType), typeof(ToType),lifetimeManager);

            _services.Add(new ServiceInfo(typeof(FromType), typeof(ToType), null));

            return container;
        }

        /// <summary>
        /// Register the type with the container and keep the registration in the list for replication
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IUnityContainer RegisterType(Type type, LifetimeManager lifetimeManager=null)
        {
            

            IUnityContainer container;
            if (lifetimeManager == null)
                container = _container.RegisterType(type);
            else
                container = _container.RegisterType(type,lifetimeManager);

            _services.Add(new ServiceInfo(type, null, null));

            return container;
            
        }

        /// <summary>
        /// Register the type with the container and keep the registration in the list for replication
        /// </summary>
        /// <param name="fromType">Convert from this type</param>
        /// <param name="toType">to this type</param>
        /// <returns>returns the container</returns>
        public IUnityContainer RegisterType(Type fromType, Type toType, LifetimeManager lifetimeManager = null)
        {
            IUnityContainer container;
            if (lifetimeManager == null)
                _container.RegisterType(fromType, toType);
            else
                _container.RegisterType(fromType, toType,lifetimeManager);


            _services.Add(new ServiceInfo(fromType,toType, null));
            return _container;

        }

        /// <summary>
        /// Register an instance of a type to be replicated
        /// </summary>
        /// <typeparam name="T">Register this type</typeparam>
        /// <param name="instance">the actual instance of the type to register</param>
        /// <returns>returns IUnityContainer</returns>
        public IUnityContainer RegisterInstance<T>(T instance)
        {
            IUnityContainer container=_container.RegisterInstance<T>(instance);
            
            _services.Add(new ServiceInfo(typeof(T),null,instance));
            return _container;
        }


        /// <summary>
        /// Register an instance of a type to be replicated
        /// </summary>
        /// <param name="type">The type of the object</param>
        /// <param name="instance">The instance of the object</param>
        /// <returns>The container</returns>
        public IUnityContainer RegisterInstance(Type type, object instance)
        {
            IUnityContainer container = _container.RegisterInstance(type,instance);

            _services.Add(new ServiceInfo(type, null, instance));
            return _container;

        }

        /// <summary>
        /// Adds an instance of an existing object to be replicated to new contexts
        /// </summary>
        /// <param name="type">The type to replicate</param>
        /// <param name="instance">The instance of the type to replicate</param>
        /// <returns>The container</returns>
        public IUnityContainer ReplicateInstance(Type type, object instance)
        {
            _services.Add(new ServiceInfo(type, null, instance));
            return _container;

        }

        /// <summary>
        /// Replicate the instance of type T when new contexts are created
        /// </summary>
        /// <typeparam name="T">The type of the instance to replicate</typeparam>
        /// <param name="instance">The instance to be returned for the specified type</param>
        /// <returns>The container</returns>
        public IUnityContainer ReplicateInstance<T>(T instance)
        {

            _services.Add(new ServiceInfo(typeof(T), null, instance));
            return _container;
        }



        /// <summary>
        /// Replicate all of the registerer services to the target container
        /// </summary>
        /// <param name="targetContainer">Adds instances of registered services to this container</param>
        /// <returns>The target container</returns>
        public IUnityContainer ReplicateToContainer(IUnityContainer targetContainer)
        {

            foreach (ServiceInfo service in _services)
            {
                if (service.Instance != null)
                {
                    targetContainer.RegisterInstance(service.FromType, service.Instance);

                } else if (service.FromType!=null) {
                    try
                    {

                        object instance=_container.Resolve(service.FromType);
                        if (instance!=null)
                            targetContainer.RegisterInstance(service.FromType,instance);
                    }
                    catch
                    {
                        //TODO:Log not able to register service
                    }

                }


            }

            return targetContainer;

        }



    }
}
