/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
namespace AppCan.Core.Services
{
    /// <summary>
    /// The interface for the service replicator.  The service replicator registers types with the root container and will replicate to all context any items that are registered
    /// </summary>
    public interface IServiceReplicator
    {
        /// <summary>
        /// Register an instance to be replicated
        /// </summary>
        /// <param name="type">The type to be replicated</param>
        /// <param name="instance">The instance that will be replicated</param>
        /// <returns>The root container</returns>
        Microsoft.Practices.Unity.IUnityContainer RegisterInstance(Type type, object instance);

        /// <summary>
        /// Register an instance that will be replicated
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="instance">The instance</param>
        /// <returns>The root container</returns>
        Microsoft.Practices.Unity.IUnityContainer RegisterInstance<T>(T instance);

        /// <summary>
        /// Register type to replicate
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <param name="lifetimeManager"></param>
        /// <returns></returns>
        Microsoft.Practices.Unity.IUnityContainer RegisterType(Type fromType, Type toType, Microsoft.Practices.Unity.LifetimeManager lifetimeManager = null);

        /// <summary>
        /// Register type to replicate
        /// </summary>
        /// <param name="type"></param>
        /// <param name="lifetimeManager"></param>
        /// <returns></returns>
        Microsoft.Practices.Unity.IUnityContainer RegisterType(Type type, Microsoft.Practices.Unity.LifetimeManager lifetimeManager = null);

        /// <summary>
        /// Register type to replicate
        /// </summary>
        /// <typeparam name="FromType"></typeparam>
        /// <typeparam name="ToType"></typeparam>
        /// <param name="lifetimeManager"></param>
        /// <returns></returns>
        Microsoft.Practices.Unity.IUnityContainer RegisterType<FromType, ToType>(Microsoft.Practices.Unity.LifetimeManager lifetimeManager = null);

        /// <summary>
        /// Regiter type to replicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lifetimeManager"></param>
        /// <returns></returns>
        Microsoft.Practices.Unity.IUnityContainer RegisterType<T>(Microsoft.Practices.Unity.LifetimeManager lifetimeManager = null);

        /// <summary>
        /// Replicate all of the registered instances to the container
        /// </summary>
        /// <param name="targetContainer">All of the registered types and instances will be replicated as instances to this target container</param>
        /// <returns>The root container</returns>
        Microsoft.Practices.Unity.IUnityContainer ReplicateToContainer(Microsoft.Practices.Unity.IUnityContainer targetContainer);
    }
}
