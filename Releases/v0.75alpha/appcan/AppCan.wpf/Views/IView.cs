/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.Core.Contexts;

namespace AppCan.wpf.Views
{
    public interface IView
    {
        IContext Context { get; set; }
        IViewModel ViewModel { get; set; }
        Object DataContext { get; set; }

    }
}
