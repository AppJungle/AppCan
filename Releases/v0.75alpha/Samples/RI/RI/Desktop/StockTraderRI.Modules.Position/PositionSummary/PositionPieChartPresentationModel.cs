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

namespace StockTraderRI.Modules.Position.PositionSummary
{
    public class PositionPieChartPresentationModel : IPositionPieChartPresentationModel,IViewModel
    {
        private readonly IPositionPieChartView view;

        public IPositionPieChartView PieView
        {
            get
            {
                return view;
            }
        }

        public IObservablePosition Position { get; private set; }

        public PositionPieChartPresentationModel(IView view, IObservablePosition observablePosition)
        {
            this.Position = observablePosition;
            View = view;
            this.view = view as IPositionPieChartView;
            this.view.ChartModel = this;
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
