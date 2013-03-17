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
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using AppCan.wpf.Views;
using AppCan.Core.Contexts;

namespace HelloWorld
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window, IViewContainer
    {
        private IContext _context;
        public Shell(IViewModel viewModel,IContext context)
        {
            _context = context;
            DataContext = viewModel;
            InitializeComponent();
         }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //var container = App.AppContext.Container; // ServiceLocator.Current.GetInstance<IUnityContainer>();
            //MyWindow win=(MyWindow)container.Resolve<ITest>();
            //win.Show();
        }

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }


        public AppCan.Core.Contexts.IContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
