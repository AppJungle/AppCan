/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppCan.wpf.Views
{
    /// <summary>
    /// Must be used by any View Model extensions.  The name will be added to the dictionary so it should be unique.
    /// </summary>
    public interface IViewModelExtension
    {
        string ExtensionName { get; }
    }
}
