//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.Windows;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;



using StockTraderRI.Modules.Market;
using StockTraderRI.Modules.News;
using StockTraderRI.Modules.Position;
using StockTraderRI.Modules.Watch;

using AppCan.wpf.Application;
using AppCan.wpf.Views;
using AppCan.Core.Contexts;
using System.Windows;
using AppCan.wpf.Contexts;

namespace StockTraderRI
{
    public partial class StockTraderRIBootstrapper : UnityBootstrapper
    {
        AppCanApp _app;
        IContext _context;
        Window _window;

        public StockTraderRIBootstrapper(AppCanApp app)
        {
            _app = app;

        }

        

        protected override void ConfigureContainer()
        {
            //Container.RegisterType<IShellView, Shell>();

            base.ConfigureContainer();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            _app.ConfigureModuleCatalog();

            ModuleCatalog catalog = (ModuleCatalog)this.ModuleCatalog;
            catalog.AddModule(typeof(MarketModule))
                .AddModule(typeof(PositionModule), "MarketModule")
                .AddModule(typeof(WatchModule), "MarketModule")
                .AddModule(typeof(NewsModule));
            //moduleCatalog.AddModule(typeof(HelloWorldModule.HelloWorldModule));
        }

        protected override DependencyObject CreateShell()
        {

            _app.CreateShell(null);

            //_app.ContextManager.LoadConfiguration("ContextDefs.xml");
            _app.ContextManager.GetNewOrExistingContextDef("StockTrader.Shell").Container.RegisterType<IViewModel, ShellPresenter>(new ContainerControlledLifetimeManager());
            _app.ContextManager.GetNewOrExistingContextDef("StockTrader.Shell").Container.RegisterType<IViewContainer, Shell>(new ContainerControlledLifetimeManager());
            //_app.ContextManager.GetNewOrExistingContextDef("StockTrader.Shell").Container.

            //IWpfContextManager wcm = _app.ContextManager as IWpfContextManager;
            //wcm.GetNewOrExistingContextDef("StockTrader.Shell", AppCan.wpf.Application.WpfContextControllerCreationOp.ViewModelFirst);

            IContext ct = _app.ContextManager.GetNewOrExistingContextDef("StockTrader.Shell").GetNewContext();
            _context = ct;

            IView view=ct.Container.Resolve<IViewModel>().View;
            _window = view as Window;

            return _window; 

            /*ShellPresenter presenter = Container.Resolve<ShellPresenter>();
            IShellView view = presenter.View;

            view.ShowView();

            return view as DependencyObject; */
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            _context.Create();
            App.Current.MainWindow = (Window)this.Shell;
            //Application.Current.MainWindow.Show();

        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            _app.InitializeShell();

            




            //App.Current.MainWindow = (Window)this.Shell;
            //App.Current.MainWindow.Show();
        }
    }
}
