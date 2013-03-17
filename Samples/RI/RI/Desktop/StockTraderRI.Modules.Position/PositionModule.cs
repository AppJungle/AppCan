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
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Services;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Orders;
using AppCan.Core.Services;
using AppCan.Core.Contexts;
using AppCan.wpf.Contexts;
using AppCan.wpf.Application;
using AppCan.wpf.Views;
using AppCan.wpf.Regions;

namespace StockTraderRI.Modules.Position
{
    public class PositionModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IServiceReplicator _serviceReplicator;

        public PositionModule(IUnityContainer container, IRegionManager regionManager,IServiceReplicator serviceReplicator)
        {
            _container = container;
            _regionManager = regionManager;
            _serviceReplicator = serviceReplicator;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            IWpfContextManager contextManager = _container.Resolve<IContextManager>() as IWpfContextManager;
            IContextDef context = contextManager.GetNewOrExistingContextDef("StockTrader.Shell");
            IContextRegionManager crm = context.Container.Resolve<IContextRegionManager>();

            //Register the view with the region, this will cause the StockTrader.TrendlineView context to be created
            crm.RegisterViewWithRegion<IViewModel>(RegionNames.ResearchRegion, "StockTrader.PositionModule.PieChart");
            crm.RegisterViewWithRegion<IViewModel>(RegionNames.MainRegion, "StockTrader.PositionModule.Summary");

            //this._regionManager.RegisterViewWithRegion(RegionNames.ResearchRegion, () => _container.Resolve<IPositionPieChartPresentationModel>().PieView);
            //this._regionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => _container.Resolve<IPositionSummaryPresentationModel>().PositionView);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This method registers all classes in the module. It's allowed to be coupled to them. ")]
        protected void RegisterViewsAndServices()
        {
            //service replicator registers with the root container and will replicate to each context
            IServiceReplicator serviceReplicator=_container.Resolve<IServiceReplicator>();

            //_container.RegisterType<IAccountPositionService, AccountPositionService>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<IOrdersService, XmlOrdersService>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<IObservablePosition, ObservablePosition>();

            serviceReplicator.RegisterType<IAccountPositionService, AccountPositionService>(new ContainerControlledLifetimeManager());
            serviceReplicator.RegisterType<IOrdersService, XmlOrdersService>(new ContainerControlledLifetimeManager());
            serviceReplicator.RegisterType<IObservablePosition, ObservablePosition>();

            IWpfContextManager contextManager = _container.Resolve<IContextManager>() as IWpfContextManager;

            // COnfigure the article context
            // configure the context to be ViewModelFirst
            

            //***********************
            //Context PositionSummary
            //Position summary view, orders controller and Observable position
            //Needs IAccountPositionService and StockTraderRICommandProxy
            IContextDef cdPosSummary=contextManager.GetNewOrExistingContextDef("StockTrader.PositionModule.Summary", WpfContextControllerCreationOp.ViewModelFirst);
            cdPosSummary.Container.RegisterType<IOrdersController, OrdersController>(new ContainerControlledLifetimeManager());  //should this be in the context or the service replicator
            cdPosSummary.Container.RegisterType<IView, PositionSummaryView>();
            cdPosSummary.Container.RegisterType<IViewModel, PositionSummaryPresentationModel>();
            cdPosSummary.Container.RegisterType<IOrderCommandsView, OrderCommandsView>();
            cdPosSummary.Container.RegisterType<IOrderCompositeView, OrderCompositeView>();
            cdPosSummary.Container.RegisterType<IOrderCompositePresentationModel, OrderCompositePresentationModel>();
            cdPosSummary.Container.RegisterType<IOrdersView, OrdersView>();
            cdPosSummary.Container.RegisterType<IOrdersPresentationModel, OrdersPresentationModel>();
            cdPosSummary.Container.RegisterType<IOrderDetailsView, OrderDetailsView>();
            cdPosSummary.Container.RegisterType<IOrderDetailsPresentationModel, OrderDetailsPresentationModel>();
            
            //_container.RegisterType<IOrdersController, OrdersController>(new ContainerControlledLifetimeManager());  //should this be in the context or the service replicator
            //_container.RegisterType<IPositionSummaryView, PositionSummaryView>();
            //_container.RegisterType<IObservablePosition, ObservablePosition>();
            //_container.RegisterType<IPositionSummaryPresentationModel, PositionSummaryPresentationModel>();
            //***********************

            //***********************
            //Context PieChart
            //Needs observable position for the presentation model
            IContextDef cdPieChart = contextManager.GetNewOrExistingContextDef("StockTrader.PositionModule.PieChart", WpfContextControllerCreationOp.ViewModelFirst);
            cdPieChart.Container.RegisterType<IView, PositionPieChartView>();
            cdPieChart.Container.RegisterType<IViewModel, PositionPieChartPresentationModel>();

            //_container.RegisterType<IPositionPieChartView, PositionPieChartView>();
            //_container.RegisterType<IPositionPieChartPresentationModel, PositionPieChartPresentationModel>();
            //***********************


            //********************
            // Context OrdersView  These also were needed by the orders controller
            //IContextDef cdOrdersView = contextManager.GetNewOrExistingContextDef("StockTrader.PositionModule.OrdersView", WpfContextControllerCreationOp.ViewModelFirst);
            //cdOrdersView.Container.RegisterType<IView, OrdersView>();
            //cdOrdersView.Container.RegisterType<IViewModel, OrdersPresentationModel>();
            //_container.RegisterType<IOrdersView, OrdersView>();
            //Needs IOrdersView
            //_container.RegisterType<IOrdersPresentationModel, OrdersPresentationModel>();
            //********************

            //********************
            //OrderDetails  //again needed to be all in the same context
            //IOrderDetailsView, IAccountPositionService, IOrdersService
            ///IContextDef cdOrdersDetailsView = contextManager.GetNewOrExistingContextDef("StockTrader.PositionModule.OrdersDetailsView", WpfContextControllerCreationOp.ViewModelFirst);
            //cdOrdersDetailsView.Container.RegisterType<IOrderDetailsView, OrderDetailsView>();
            //cdOrdersDetailsView.Container.RegisterType<IOrderDetailsPresentationModel, OrderDetailsPresentationModel>();
            
            //_container.RegisterType<IOrderDetailsView, OrderDetailsView>();
            //_container.RegisterType<IOrderDetailsPresentationModel, OrderDetailsPresentationModel>();


            
            //Avoided another context since the orders controller context needed access to the composite presentation mode
            // which then needed the commands and composite view
            //Context CompositeView
            //IOrderCompositeView, IOrderDetailsPresentationModel, IOrderCommandsView
            //IContextDef cdOrderCompositeView = contextManager.GetNewOrExistingContextDef("StockTrader.PositionModule.CompositeView", WpfContextControllerCreationOp.ViewModelFirst);
            //cdOrderCompositeView.Container.RegisterType<IOrderCommandsView, OrderCommandsView>();
            //cdOrderCompositeView.Container.RegisterType<IOrderCompositeView, OrderCompositeView>();
            //cdOrderCompositeView.Container.RegisterType<IOrderCompositePresentationModel, OrderCompositePresentationModel>();

            //_container.RegisterType<IOrderCommandsView, OrderCommandsView>();
            //_container.RegisterType<IOrderCompositeView, OrderCompositeView>();
            //_container.RegisterType<IOrderCompositePresentationModel, OrderCompositePresentationModel>();
            
            
        }
    }
}
