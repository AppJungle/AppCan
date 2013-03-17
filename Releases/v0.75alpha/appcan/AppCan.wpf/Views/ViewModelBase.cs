/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.wpf.Views;
using AppCan.Core.Application;
using Microsoft.Practices.Prism.Regions;

namespace AppCan.wpf.Views
{
    /// <summary>
    /// Optional base class for your view models as a helper to implement the properties needed for IViewModel
    /// </summary>
    public class ViewModelBase :IViewModel
    {
        protected IRegionManager _localRegionManager;
        protected IModelContainer _modelContainer;
        protected IModel _model;
        protected IView _view;
         
        /// <summary>
        /// Constructor takes a local region manager
        /// </summary>
        /// <param name="localRegionManager">The local region manager</param>
        public ViewModelBase(IRegionManager localRegionManager)
        {

            _localRegionManager = localRegionManager;
        }

        /// <summary>
        /// The model container associated with this
        /// </summary>
        public IModelContainer ModelContainer
        {
            get
            {
                return _modelContainer;
            }
            set
            {
                _modelContainer=value;
            }
        }


        /// <summary>
        /// The model associated with this
        /// </summary>

        public AppCan.Core.Application.IModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model=value;
            }
        }

        /// <summary>
        /// The view or null
        /// </summary>
        public IView View
        {
            get
            {
                return _view;
            }
            set
            {
                _view=value;
            }
        }

        /// <summary>
        /// View model extensions if any
        /// </summary>
        public virtual Dictionary<string, IViewModelExtension> Extensions { get; set; }

        /// <summary>
        /// The local region manager
        /// </summary>
        public virtual IRegionManager LocalRegionManager { get { return _localRegionManager; } }
    }
}
