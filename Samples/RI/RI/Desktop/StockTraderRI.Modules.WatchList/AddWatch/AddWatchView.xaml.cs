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
using System.Windows.Controls;
using System.Windows.Input;
using AppCan.wpf.Views;

namespace StockTraderRI.Modules.Watch.AddWatch
{
    /// <summary>
    /// Interaction logic for AddWatchControl.xaml
    /// </summary>
    public partial class AddWatchView : UserControl, IAddWatchView,IView

    {
        public AddWatchView()
        {
            InitializeComponent();
        }

        #region IAddWatchView Members

        public void SetAddWatchCommand(ICommand addWatchCommand)
        {
            this.DataContext = new AddWatchPresentationModel { AddWatchCommand = addWatchCommand };
        }

        #endregion

        public AppCan.Core.Contexts.IContext Context
        {
            get;
            
            set;
            
        }

        public IViewModel ViewModel
        {
            get;
            
            set;
            
        }
    }

    public class AddWatchPresentationModel : INotifyPropertyChanged
    {
        public ICommand AddWatchCommand { get; set; }

        private string stockSymbol;

        public string StockSymbol
        {
            get { return stockSymbol; }
            set
            {
                stockSymbol = value;
                OnPropertyChanged("StockSymbol");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler Handler = PropertyChanged;
            if (Handler != null) Handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
