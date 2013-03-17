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
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using AppCan.Core.Contexts;
using AppCan.wpf.Views;
using Microsoft.Practices.ServiceLocation;
using HelloWorldModule.Views;
using AppCan.wpf.Regions;

namespace HelloWorldModule
{
    public class HelloWorldModule : IModule
    {
        private readonly IRegionViewRegistry regionViewRegistry;

        public HelloWorldModule(IRegionViewRegistry registry)
        {
            this.regionViewRegistry = registry;   
        }

        public void Initialize()
        {
            
            //Get the container
            //IUnityContainer container=ServiceLocator.Current.GetInstance<IUnityContainer>();
            
            //Get the context manager
            //IContextManager cm = container.Resolve<IContextManager>();

            //Get the context definition
            //IContextDef cd=cm.GetNewOrExistingContextDef("HelloWorld.Shell.MenuRegion");

            //Register the view to the context definition 
            //cd.Container.RegisterType<IView, MenuView>();

            //Register the view model to the context definition
            //cd.Container.RegisterType<IViewModel, HellowWorldViewModel>();

            //IContextDef cdHelloWorldShell = cm.GetNewOrExistingContextDef("HelloWorld.Shell");

            //Get the context region manager from the context definition 
            //IContextRegionManager crm = cdHelloWorldShell.Container.Resolve<IContextRegionManager>();
            
            //- we will pre-register that the IView that we registered we want to go to a particular region
            //When the context definition is created the regions will be created into the context specified.
            //This example registers a IView with a region in a different context from where the IView will be created
            //This allows us to use the generic IView rather than needing to provide a class specific interface
            //crm.RegisterViewWithRegion<IView>("MenuRegion", "HelloWorld.Shell.MenuRegion");
            
            
        }
    }
}
