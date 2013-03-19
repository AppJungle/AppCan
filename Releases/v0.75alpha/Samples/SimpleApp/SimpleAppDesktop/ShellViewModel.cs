using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.Core.Contexts;
using AppCan.wpf.Application;
using AppCan.wpf.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Prism.Regions;
using AppCan.Core.Application;

namespace HelloWorld
{
    public class ShellViewModel : IViewModel
    {
        private IRegionManager _localRegionManager;
        IModelManager _modelManager;
        IModel _model;

        public ShellViewModel(IRegionManager localRegionManager, IContext context)
        {

            _localRegionManager = localRegionManager;
            _modelManager=context.RootContainer.Resolve<IModelManager>();
            _model = _modelManager.FirstModelContainer.Model;
        }

        public AppCan.Core.Application.IModelContainer ModelContainer
        {
            get;
           
            set;
            
        }

        public AppCan.Core.Application.IModel Model
        {
            get;
            
            set;
            
        }

        public IView View
        {
            get;
            
            set;
           
        }

        public Dictionary<string, IViewModelExtension> Extensions { get; set; }

        public IRegionManager LocalRegionManager { get { return _localRegionManager;  } }
    }
}
