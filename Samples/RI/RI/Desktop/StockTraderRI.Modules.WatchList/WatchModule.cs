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
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Watch.AddWatch;
using StockTraderRI.Modules.Watch.Controllers;
using StockTraderRI.Modules.Watch.Services;
using StockTraderRI.Modules.Watch.WatchList;
using AppCan.Core.Services;
using AppCan.Core.Contexts;
using AppCan.wpf.Contexts;
using AppCan.wpf.Application;
using AppCan.wpf.Views;
using AppCan.wpf.Regions;

namespace StockTraderRI.Modules.Watch
{
    public class WatchModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public WatchModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            this.RegisterViewsAndServices();

            var controller = this._container.Resolve<IWatchListController>();
            controller.Run();

            IWpfContextManager contextManager = _container.Resolve<IContextManager>() as IWpfContextManager;
            IContextDef context = contextManager.GetNewOrExistingContextDef("StockTrader.Shell");
            IContextRegionManager crm = context.Container.Resolve<IContextRegionManager>();

            //Register the view with the region, this will cause the StockTrader.TrendlineView context to be created
            crm.RegisterViewWithRegion<IViewModel>(RegionNames.MainToolBarRegion, "StockTrader.WatchModule.View");

            //this._regionManager.RegisterViewWithRegion(
              //                                      RegionNames.MainToolBarRegion,
                //                                    () => this._container.Resolve<IAddWatchPresenter>().WatchView);
        }

        protected void RegisterViewsAndServices()
        {
            IWpfContextManager contextManager = _container.Resolve<IContextManager>() as IWpfContextManager;
            IServiceReplicator serviceReplicator = _container.Resolve<IServiceReplicator>();

            //_container.RegisterType<IWatchListService, WatchListService>(new ContainerControlledLifetimeManager());
            serviceReplicator.RegisterType<IWatchListService, WatchListService>(new ContainerControlledLifetimeManager());

            //TODO: Refactor this code to be contained in a context
            _container.RegisterType<IWatchListController, WatchListController>();
            _container.RegisterType<IWatchListView, WatchListView>();
            _container.RegisterType<IWatchListPresentationModel, WatchListPresentationModel>();

            IContextDef cdWatchView = contextManager.GetNewOrExistingContextDef("StockTrader.WatchModule.View", WpfContextControllerCreationOp.ViewModelFirst);

            cdWatchView.Container.RegisterType<IView, AddWatchView>();
            cdWatchView.Container.RegisterType<IViewModel, AddWatchPresenter>();

            //_container.RegisterType<IAddWatchView, AddWatchView>();
            //_container.RegisterType<IAddWatchPresenter, AddWatchPresenter>();
        }

        #endregion
    }
}
