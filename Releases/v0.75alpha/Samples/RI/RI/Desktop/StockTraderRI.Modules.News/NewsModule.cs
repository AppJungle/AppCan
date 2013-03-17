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



using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.Services;

using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;


using AppCan.wpf.Views;
using AppCan.Core.Contexts;
using AppCan.wpf.Contexts;
using AppCan.wpf.Regions;
using Microsoft.Practices.Prism.Events;

namespace StockTraderRI.Modules.News
{
    public class NewsModule : IModule
    {
        private readonly IUnityContainer container;
        private IView _newsView;
        private IRegion _secondaryRegion;
        private IEventAggregator _eventAggregator;

        public NewsModule(IUnityContainer container,IEventAggregator eventAggregator)
        {
            this.container = container;
            _eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            this.RegisterViewsAndServices();

            INewsController controller = this.container.Resolve<INewsController>();
            controller.Run();
        }

        protected void RegisterViewsAndServices()
        {
            IWpfContextManager contextManager = container.Resolve<IContextManager>() as IWpfContextManager;

            // COnfigure the article context
            // configure the context to be ViewModelFirst
            contextManager.GetNewOrExistingContextDef("StockTrader.Article", AppCan.wpf.Application.WpfContextControllerCreationOp.ViewModelFirst).Container.RegisterType<IView, ArticleView>();
            contextManager.GetNewOrExistingContextDef("StockTrader.Article").Container.RegisterType<IViewModel, ArticlePresentationModel>();

            //Configure the news context
            contextManager.GetNewOrExistingContextDef("StockTrader.News", AppCan.wpf.Application.WpfContextControllerCreationOp.ViewModelFirst).Container.RegisterType<IView, NewsReader>();
            contextManager.GetNewOrExistingContextDef("StockTrader.News").Container.RegisterType<IViewModel, NewsReaderPresenter>();

            this.container.RegisterType<INewsController, NewsController>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<INewsFeedService, NewsFeedService>(new ContainerControlledLifetimeManager());

            IRegionManager rm = container.Resolve<IRegionManager>();

            //Create a news context and add the view to the research region
            IContextDef cd=contextManager.GetNewOrExistingContextDef("StockTrader.News");
            IContext contextNews=cd.GetNewContext();
            IViewModel vm = contextNews.Container.Resolve<IViewModel>();
            rm.Regions[RegionNames.SecondaryRegion].Add(vm.View);

            _secondaryRegion = rm.Regions[RegionNames.SecondaryRegion];
            _newsView = vm.View;


            //Create an article context and set the view into the research region
            IContextDef cdArticle = contextManager.GetNewOrExistingContextDef("StockTrader.Article");
            IContext contextArticle = cdArticle.GetNewContext();
            IViewModel vmArticle = contextArticle.Container.Resolve<IViewModel>();
            rm.Regions[RegionNames.ResearchRegion].Add(vmArticle.View);

            _eventAggregator.GetEvent<ActivateNewsViewEvent>().Subscribe(ActivateNewsView, ThreadOption.UIThread,true);

        }

        /// <summary>
        /// Activate the news view
        /// </summary>
        /// <param name="none"></param>
        public void ActivateNewsView(object none)
        {
            _secondaryRegion.Activate(_newsView);
        }
    }
}
