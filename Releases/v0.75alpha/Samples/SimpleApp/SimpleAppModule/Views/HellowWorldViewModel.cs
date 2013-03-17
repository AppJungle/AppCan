using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.wpf.Views;
using AppCan.Core.Application;
using AppCan.Core.Contexts;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel;

namespace HelloWorldModule.Views
{
    

    public class HelloWorldViewModel : IViewModel, INotifyPropertyChanged 
    {
        private IRegionManager _localRegionManager;
        private IModelManager _modelManager;
        private IModel _model;

        public HelloWorldViewModel(IRegionManager localRegionManager,IContext context)
        {

            _localRegionManager=localRegionManager;
            _modelManager = context.RootContainer.Resolve<IModelManager>();
            _model = _modelManager.FirstModelContainer.Model;
            _modelManager.Opened += new ModelEventHandler(ModelManager_Opened);
        }

        void ModelManager_Opened(object sender, ModelEventArgs e)
        {
            _model = _modelManager.FirstModelContainer.Model;
            OnPropertyChanged("Model");
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public AppCan.Core.Application.ModelContainer ModelContainer
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
                return _model;
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
