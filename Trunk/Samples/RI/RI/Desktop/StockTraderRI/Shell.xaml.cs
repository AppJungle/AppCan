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
using System.Windows;
using AppCan.wpf.Views;
using AppCan.Core.Contexts;

namespace StockTraderRI
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window, IShellView, IViewContainer
    {
        private IContext _context;
        private IViewModel _viewModel;

        public Shell()
        {
            InitializeComponent();
        }

        public Shell(IContext context)
        {
            _context = context;
            InitializeComponent();
        }

        public void ShowView()
        {
            this.Show();
        }

        public AppCan.Core.Contexts.IContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context=value;
            }
        }

        public IViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }
    }
}
