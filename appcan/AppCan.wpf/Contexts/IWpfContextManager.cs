/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.Core.Contexts;
using AppCan.wpf.Application;

namespace AppCan.wpf.Contexts
{
    /// <summary>
    /// Interface for a WpfContextManager.
    /// </summary>
    public interface IWpfContextManager  : IContextManager
    {
        /// <summary>
        /// Wpf specific version that allows specifying the creation op to use
        /// </summary>
        /// <param name="contextName">The name of the context</param>
        /// <param name="creationOp">None, ViewFirst (creates IView first), ViewModelFirst (creates IViewModel first)</param>
        /// <returns></returns>
        IContextDef GetNewOrExistingContextDef(string contextName,WpfContextControllerCreationOp creationOp);
    }
}
