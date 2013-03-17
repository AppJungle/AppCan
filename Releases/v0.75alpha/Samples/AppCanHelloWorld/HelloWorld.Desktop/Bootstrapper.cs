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


using AppCan.Core.Contexts;
using AppCan.wpf.Application;
using AppCan.wpf.Views;
using log4net;
using log4net.Config;
using Microsoft.Practices.Prism.Logging;

namespace HelloWorld
{
    class Bootstrapper : UnityBootstrapper
    {
        Context _context;
        Window _window;
        App _app;

        public Bootstrapper(App app)
        {
            _app = app;

            XmlConfigurator.Configure(new System.IO.FileInfo("Log4net.xml"));

        }

        


        protected override DependencyObject CreateShell()
        {
            _app.CreateShell(null);

            _app.ContextManager.LoadConfiguration("ContextDefs.xml");
            //_app.ContextManager.GetNewOrExistingContextDef(ContextNames.HelloWorldShell).Container.RegisterType<IViewModel, ShellViewModel>(new ContainerControlledLifetimeManager());
            //_app.ContextManager.GetNewOrExistingContextDef(ContextNames.HelloWorldShell).Container.RegisterType<IViewContainer, Shell>(new ContainerControlledLifetimeManager());
            
            
            Context ct =_app.ContextManager.GetNewOrExistingContextDef(ContextNames.HelloWorldShell).GetNewContext();
            _context = ct;

            _window = (Window)ct.Container.Resolve<IViewContainer>();

            return _window;

            //return this.Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            _app.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;

                        
            

            //App.Current.MainWindow = (Window)this.Shell;
            //App.Current.MainWindow.Show();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            _context.Create();
            //Application.Current.MainWindow.Show();

        }


        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            _app.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(HelloWorldModule.HelloWorldModule));
        }

        //private readonly Log4NetLogger _logger = new Log4NetLogger();
        protected virtual ILoggerFacade CreateLogger()
        {
            return new Log4NetLogger();
        }

        



        
    }
}
