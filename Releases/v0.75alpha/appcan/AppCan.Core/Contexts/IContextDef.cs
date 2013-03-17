/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
namespace AppCan.Core.Contexts
{
    /// <summary>
    /// Interface for ContextDef (context definitions).  This is used to configure the context with objects in it's container that will 
    /// be used/created in contexts that are created from this context definition
    /// </summary>
    public interface IContextDef
    {
        /// <summary>
        /// The container - Add items to this container that you want to be available in the context's that are created
        /// </summary>
        Microsoft.Practices.Unity.IUnityContainer Container { get; }

        /// <summary>
        /// The contexts created by this context definition.
        /// </summary>
        
        List<KeyValuePair<int, WeakReference>> Contexts { get; }

        /// <summary>
        /// Get a new context
        /// </summary>
        /// <returns></returns>
        Context GetNewContext();

        /// <summary>
        /// Get a new context that is a child of another context
        /// </summary>
        /// <param name="parentContext">The parent context</param>
        /// <returns></returns>
        Context GetNewContext(IContext parentContext);
    }
}
