/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppCan.Core.Contexts
{
    /// <summary>
    /// Context controll interface allows using a new instance or same instance of the controller for all contexts.
    /// The controller is sometimes responsible for creating the instances in the context, the way the application desires and activating 
    /// a context if the context already exists and was previously released.
    /// Lifetime is Init->Create->(Activate)->Release for a context.
    /// </summary>
    public interface IContextController
    {
        /// <summary>
        /// Called when a context definition is created
        /// </summary>
        /// <param name="context"></param>
        void ContextDefInit(IContextDef context);  //When first added to a context def or context def created

        /// <summary>
        /// Called when a new context is created
        /// </summary>
        /// <param name="context"></param>
        void Init(IContext context);  // called when constructed

        /// <summary>
        /// Called when the context is to create it's windows/views
        /// </summary>
        /// <param name="context"></param>
        void Create(IContext context);  //called when the context is created

        /// <summary>
        /// Called to Activate/re-activate
        /// </summary>
        /// <param name="context"></param>
        void Activate(IContext context); // called when the context is activated

        /// <summary>
        /// Called when the context is released
        /// </summary>
        /// <param name="context"></param>
        void Release(IContext context);  //called when the context is released

    }
}
