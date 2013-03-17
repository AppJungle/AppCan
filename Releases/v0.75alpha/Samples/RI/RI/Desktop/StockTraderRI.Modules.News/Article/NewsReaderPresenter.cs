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
using StockTraderRI.Infrastructure.Models;
using AppCan.wpf.Views;
using AppCan.Core.Contexts;
using Microsoft.Practices.Prism.Events;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace StockTraderRI.Modules.News.Article
{
    public class NewsReaderPresenter : INewsReaderPresenter, IViewModel
    {
        private readonly INewsReaderView view;

        public NewsReaderPresenter(IView view,IContext context)
        {
            this.view = view as INewsReaderView;
            View = view;
            IEventAggregator eventAggregator=context.RootContainer.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<SetNewsArticleEvent>().Subscribe(SetNewsArticle, ThreadOption.UIThread,true);
        }

        public INewsReaderView NewsView
        {
            get { return this.view; }
        }

        public void SetNewsArticle(NewsArticle article)
        {
            this.view.NewsModel = article;
        }

        public System.Collections.Generic.Dictionary<string, IViewModelExtension> Extensions
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public Microsoft.Practices.Prism.Regions.IRegionManager LocalRegionManager
        {
            get { throw new System.NotImplementedException(); }
        }

        public AppCan.Core.Application.IModel Model
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public AppCan.Core.Application.IModelContainer ModelContainer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public IView View
        {
            get;
            
            set;
            
        }
    }
}
