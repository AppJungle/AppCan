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
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Market.Services;
using StockTraderRI.Modules.Market.TrendLine;
using AppCan.Core.Contexts;
using AppCan.wpf.Regions;
using AppCan.wpf.Contexts;
using AppCan.wpf.Views;

namespace StockTraderRI.Modules.Market
{
    public class MarketModule : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IWpfContextManager _contextManager;

        public MarketModule(IUnityContainer container, IRegionManager regionManager,IContextManager contextManager)
        {
            this.container = container;
            this.regionManager = regionManager;
            _contextManager = contextManager as IWpfContextManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();

        }

        protected void RegisterViewsAndServices()
        {
            container.RegisterType<IMarketHistoryService, MarketHistoryService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMarketFeedService, MarketFeedService>(new ContainerControlledLifetimeManager());

            
            // configure the context to be ViewModelFirst
            _contextManager.GetNewOrExistingContextDef("StockTrader.TrendlineView",AppCan.wpf.Application.WpfContextControllerCreationOp.ViewModelFirst);

            
            _contextManager.GetNewOrExistingContextDef("StockTrader.TrendlineView").Container.RegisterType<IView, TrendLineView>();
            _contextManager.GetNewOrExistingContextDef("StockTrader.TrendlineView").Container.RegisterType<IViewModel, TrendLinePresentationModel>();
            
            //Get the root context definitions region manager since the region exists in the root context
            IContextDef context=_contextManager.GetNewOrExistingContextDef("StockTrader.Shell");
            IContextRegionManager crm=context.Container.Resolve<IContextRegionManager>();

            //Register the view with the region, this will cause the StockTrader.TrendlineView context to be created
            crm.RegisterViewWithRegion<IViewModel>(RegionNames.ResearchRegion,"StockTrader.TrendlineView");
            
            
        }

        #endregion
    }
}
