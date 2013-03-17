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
using AppCan.Core.Contexts;
using System.Configuration;
using AppCan.wpf.Application;

using AppCan.Core.Application;
using AppCan.wpf.Views;

//using Microsoft.Practices.Unity.Configuration;

namespace HelloWorld
{
    

    



    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : AppCanApp
    {
        public static Context AppContext;

        protected override void OnStartup(StartupEventArgs e)
        {
           App.ModelManager.AddModelTemplate(new ModelTemplate(typeof(NoteModel), ".can", "NewFile{0}.can", new BinarySerializer(), true, typeof(Shell), "/HelloWorld;component/Shell.xaml", "HelloWorld.mainwindow"));

            
            base.OnStartup(e);


            Bootstrapper bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();

    

        }
    }
}
