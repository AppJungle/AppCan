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
using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using AppCan.wpf.Application;

namespace StockTraderRI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : AppCanApp
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if (DEBUG)
            RunInDebugMode(this);
#else
            RunInReleaseMode(this);
#endif
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private static void RunInDebugMode(AppCanApp app)
        {
            UnityBootstrapper bootstrapper = new StockTraderRIBootstrapper(app);
            bootstrapper.Run();
        }

        private static void RunInReleaseMode(AppCanApp app)
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            try
            {
                UnityBootstrapper bootstrapper = new StockTraderRIBootstrapper(app);
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            ExceptionPolicy.HandleException(ex, "Default Policy");
            MessageBox.Show(StockTraderRI.Properties.Resources.UnhandledException);
            Environment.Exit(1);
        }
    }
}