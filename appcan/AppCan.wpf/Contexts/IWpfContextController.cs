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
    /// Interface to a Wpf context controller
    /// </summary>
    public interface IWpfContextController : IContextController
    {
        /// <summary>
        /// The creation operation to use for the context, None, ViewFirst (Creates the IView first), ViewModelFirst (creates the IViewModel class first)
        /// </summary>
        WpfContextControllerCreationOp CreationOp { get; set;} 
        
    }
}
