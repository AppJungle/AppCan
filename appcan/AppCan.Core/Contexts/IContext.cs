/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace AppCan.Core.Contexts
{
    /// <summary>
    /// Interface for Context's
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// The contaienr
        /// </summary>
        IUnityContainer Container { get; }

        /// <summary>
        /// The root container
        /// </summary>
        IUnityContainer RootContainer { get; }

        /// <summary>
        /// The contex definition that created this context
        /// </summary>
        IContextDef ContextDef { get; }

        /// <summary>
        /// The parent context or Null
        /// </summary>
        IContext ParentContext { get; }

        /// <summary>
        /// Used to activate/reshow a context
        /// </summary>
        void Activate();

        /// <summary>
        /// Create/show a context
        /// </summary>
        void Create();

        /// <summary>
        /// When a context is first created this is called.
        /// </summary>
        void Init();
        


        
    }
}
