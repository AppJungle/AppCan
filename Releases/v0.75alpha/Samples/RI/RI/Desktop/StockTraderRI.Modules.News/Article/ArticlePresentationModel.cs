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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using StockTraderRI.Infrastructure;


using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Controllers;
using AppCan.wpf.Views;
using AppCan.Core.Contexts;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace StockTraderRI.Modules.News.Article
{

    public class ArticlePresentationModel : IArticlePresentationModel, INotifyPropertyChanged,IViewModel
    {
        private IList<NewsArticle> articles;
        private NewsArticle selectedArticle;
        private readonly INewsFeedService newsFeedService;
        private IContext _context;
        private ActivateNewsViewEvent _activateNewsViewEvent;
        private SetNewsArticleEvent _setNewsArticleEvent;

        public event PropertyChangedEventHandler PropertyChanged;


        public ArticlePresentationModel(IView view,  IContext context)
        {
            INewsFeedService newsFeedService = context.RootContainer.Resolve<INewsFeedService>();

            _context = context;
            ArticleView = view as IArticleView;
            View = view;
            ArticleView.AViewModel = this;
            this.newsFeedService = newsFeedService;
            ArticleView.ShowNewsReader += View_ShowNewsReader;
            IEventAggregator eventAggregator=_context.RootContainer.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<SetTickerSymbolEvent>().Subscribe(SetTickerSymbol, ThreadOption.UIThread);
            _activateNewsViewEvent=eventAggregator.GetEvent<ActivateNewsViewEvent>();
            _setNewsArticleEvent = eventAggregator.GetEvent<SetNewsArticleEvent>();
        }

        public IArticleView ArticleView { get; private set; }

        public INewsController Controller { get; set; }

        public void SetTickerSymbol(string companySymbol)
        {
            this.Articles = newsFeedService.GetNews(companySymbol);
        }


        public NewsArticle SelectedArticle
        {
            get { return this.selectedArticle; }
            set
            {
                if (this.selectedArticle != value)
                {
                    this.selectedArticle = value;
                    OnPropertyChanged("SelectedArticle");
                    this.SelectedArticleChanged();
                }
            }
        }
        
        public IList<NewsArticle> Articles
        {
            get { return this.articles; }
            private set
            {
                if (this.articles != value)
                {
                    this.articles = value;
                    OnPropertyChanged("Articles");
                }
            }
        }

        private void View_ShowNewsReader(object sender, EventArgs e)
        {
            _activateNewsViewEvent.Publish(null);
            //this.Controller.ShowNewsReader();
        }

        private void SelectedArticleChanged()
        {
            _setNewsArticleEvent.Publish(this.SelectedArticle);
            //this.Controller.CurrentNewsArticleChanged(this.SelectedArticle);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Dictionary<string, IViewModelExtension> Extensions
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

        public Microsoft.Practices.Prism.Regions.IRegionManager LocalRegionManager
        {
            get { throw new NotImplementedException(); }
        }

        public AppCan.Core.Application.IModel Model
        {
            get;

            set;

        }

        public AppCan.Core.Application.IModelContainer ModelContainer
        {
            get;

            set;

        }

        public IView View
        {
            get;
           
            set;
            
        }
    }
}
