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
using System.ComponentModel;
using Microsoft.Practices.Prism.Events;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using AppCan.wpf.Views;
using Microsoft.Practices.Prism.Regions;
using AppCan.Core.Application;
using AppCan.Core.Contexts;
using Microsoft.Practices.Unity;

namespace StockTraderRI.Modules.Market.TrendLine
{
    public class TrendLinePresentationModel : ITrendLinePresentationModel, INotifyPropertyChanged, IViewModel
    {
        private readonly IMarketHistoryService _marketHistoryService;

        private string tickerSymbol;

        private MarketHistoryCollection historyCollection;
        private System.Collections.Generic.Dictionary<string, IViewModelExtension> _extensions = new System.Collections.Generic.Dictionary<string, IViewModelExtension>();
        private IRegionManager _regionManager;
        private IView _view;

        //ITrendLineView
        public TrendLinePresentationModel(IView view, IContext context,IRegionManager regionManager)
        {
            //IMarketHistoryService marketHistoryService, IEventAggregator eventAggregator
            IEventAggregator eventAggregator = context.RootContainer.Resolve<IEventAggregator>();
            IMarketHistoryService marketHistoryService = context.RootContainer.Resolve<IMarketHistoryService>();
            this.View = view;
            this.View.ViewModel = this;
            this.View.DataContext = this;
            this._marketHistoryService = marketHistoryService;
            eventAggregator.GetEvent<TickerSymbolSelectedEvent>().Subscribe(this.TickerSymbolChanged);
            _regionManager = regionManager;
        }

        public void TickerSymbolChanged(string newTickerSymbol)
        {
            MarketHistoryCollection newHistoryCollection = this._marketHistoryService.GetPriceHistory(newTickerSymbol);

            this.TickerSymbol = newTickerSymbol;
            this.HistoryCollection = newHistoryCollection;
        }

        #region ITrendLinePresentationModel Members

        //public ITrendLineView View { get; set; }

        public string TickerSymbol
        {
            get
            {
                return this.tickerSymbol;
            }
            set
            {
                if (this.tickerSymbol != value)
                {
                    this.tickerSymbol = value;
                    this.InvokePropertyChanged("TickerSymbol");
                }
            }
        }

        public MarketHistoryCollection HistoryCollection
        {
            get
            {
                return historyCollection;
            }
            private set
            {
                if (this.historyCollection != value)
                {
                    this.historyCollection = value;
                    this.InvokePropertyChanged("HistoryCollection");
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler Handler = PropertyChanged;
            if (Handler != null) Handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public System.Collections.Generic.Dictionary<string, IViewModelExtension> Extensions
        {
            get
            {
                return _extensions;
            }
            set
            {
                _extensions=value;
            }
        }

        public Microsoft.Practices.Prism.Regions.IRegionManager LocalRegionManager
        {
            get { return _regionManager; }
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
            get
            {
                return _view;
            }
            set
            {
                _view = value;
            }
        }
    }
}
