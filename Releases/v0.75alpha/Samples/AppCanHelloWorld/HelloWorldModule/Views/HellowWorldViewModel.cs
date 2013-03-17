using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.wpf.Views;
using AppCan.Core.Application;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Prism.Regions;

namespace HelloWorldModule.Views
{
    public class HelloWorldViewModel : IViewModel
    {
        private IRegionManager _localRegionManager;

        public HelloWorldViewModel(IRegionManager localRegionManager)
        {

            _localRegionManager=localRegionManager;
        }

        public AppCan.Core.Application.IModelContainer ModelContainer
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public AppCan.Core.Application.IModel Model
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IView View
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<string, IViewModelExtension> Extensions { get; set; }

        public IRegionManager LocalRegionManager { get { return _localRegionManager;} }     
    }
}
