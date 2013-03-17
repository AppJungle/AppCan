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
using StockTraderRI.Modules.Position.Interfaces;
using AppCan.wpf.Views;

namespace StockTraderRI.Modules.Position.Orders
{
    public partial class OrdersPresentationModel : IOrdersPresentationModel,IViewModel
    {
        public OrdersPresentationModel(IOrdersView view)
        {
            this.OrdersView = view as IOrdersView;
            this.OrdersView.SetModel(this);
        }

        public string HeaderInfo
        {
            get { return "BUY & SELL"; }
        }

        public IOrdersView OrdersView { get; set; }

        public System.Collections.Generic.Dictionary<string, IViewModelExtension> Extensions
        {
            get;
            
            set;
            
        }

        public Microsoft.Practices.Prism.Regions.IRegionManager LocalRegionManager
        {
            get { throw new System.NotImplementedException(); }
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
