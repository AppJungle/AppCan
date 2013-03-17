/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.Core.Application;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Prism.Regions;
using AppCan.Core.Contexts;


namespace AppCan.wpf.Views
{
    /// <summary>
    /// IViewContainer is used for Window classes to indicate that the window can be used as a shell
    /// </summary>
    public interface IViewContainer : IView
    {
        /// <summary>
        /// The View Model
        /// </summary>
        IViewModel ViewModel { get; set; }

        /// <summary>
        /// The context
        /// </summary>
        IContext Context { get; set; }

        //The data context (often points to the same thing as the ViewModel
        Object DataContext { get; set; }
    }
}
