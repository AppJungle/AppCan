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

namespace AppCan.wpf.Views
{
    public interface IViewModel
    {
        IModelContainer ModelContainer { get; set; }
        IModel Model { get; set; }
        IView View { get; set; }
        IRegionManager LocalRegionManager { get; }      

        /// <summary>
        /// View model extensions.  This will support adding additional view models
        /// They can be accessed from a property like this Text="{Binding ViewModelExtensions[test].MyViewModelProperty}"
        /// </summary>
        Dictionary<string, IViewModelExtension> Extensions { get; set; }
        

    }
}
