/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using AppCan.wpf.Regions;
using Microsoft.Practices.ServiceLocation;
using AppCan.Core.Contexts;
using AppCan.Core.Utils;
using AppCan.wpf.Views;
using AppCan.wpf.Contexts;
using System.Windows;


namespace AppCan.wpf.Application
{
    /// <summary>
    /// Used by the WpfContextControllerclass and IWpfContextControllerInterface
    /// </summary>
    public enum WpfContextControllerCreationOp { ViewFirst, ViewModelFirst, None }

    /// <summary>
    /// WpfContextController, used by ContextManager, COntextDefs and Contexts
    /// 
    /// </summary>
    public class WpfContextController : IWpfContextController
    {
        WpfContextControllerCreationOp _creationOp = WpfContextControllerCreationOp.ViewFirst;
        
        /// <summary>
        /// When added to a context definition this is called to init it.
        /// </summary>
        /// <param name="contextDef"></param>
        public void ContextDefInit(IContextDef contextDef)  //When first added to a context def or context def created
        {
            //contextDef.Container.RegisterInstance<IContext>(context);
            contextDef.Container.RegisterType<IContextRegionManager, ContextDefRegionManager>(new ContainerControlledLifetimeManager());

            contextDef.Container.RegisterType<IViewModelExtensionsController, ViewModelExtensionsController>(new ContainerControlledLifetimeManager());

        }

        /// <summary>
        /// Define how creation of a context can take place.  Can specify ViewFirst,ViewModelFirst or None.
        /// ViewFirst is the default.
        /// </summary>
        public WpfContextControllerCreationOp CreationOp { get { return _creationOp; } set { _creationOp = value; } } 

        public virtual void Init(Core.Contexts.IContext context)
        {
            //create a new region manager
            
            IRegionManager rm = ServiceLocator.Current.GetInstance<IRegionManager>();

            
            context.Container.RegisterInstance<IRegionManager>(rm);
            
            

        }

        /// <summary>
        /// Called when the context is created, this method will instantiate and create the IViewContext or View/ViewModel based on config
        /// </summary>
        /// <param name="context">The context</param>
        public virtual void Create(Core.Contexts.IContext context)
        {
            IRegionManager rm = context.Container.Resolve<IRegionManager>();
            IContextRegionManager crm = context.ContextDef.Container.Resolve<IContextRegionManager>();

            foreach (RegionData rd in crm.Regions)
            {
                //rm.RegisterViewWithRegion(rd.RegionName, rd.RegionType);

                if (rd.Context == null)
                {
                    if (_creationOp == WpfContextControllerCreationOp.ViewFirst)
                    {
                        rm.RegisterViewWithRegion(rd.RegionName,
                            () => context.Container.Resolve(rd.RegionType));
                    } else
                        if (_creationOp == WpfContextControllerCreationOp.ViewModelFirst)
                        {
                            rm.RegisterViewWithRegion(rd.RegionName,
                            () => (context.Container.Resolve(rd.RegionType) as IViewModel).View);

                        }

                }
                else
                {
                    
                    

                    IContextManager cm=context.RootContainer.Resolve<IContextManager>();
                    ContextDef cd=cm.GetNewOrExistingContextDef(rd.Context);
                    IContext regionContext = cd.GetNewContext(context);

                    IWpfContextController wrcc = regionContext.Container.Resolve<IContextController>() as IWpfContextController;

                    if (wrcc.CreationOp == WpfContextControllerCreationOp.ViewFirst)
                    {
                        rm.RegisterViewWithRegion(rd.RegionName,
                           () => regionContext.Container.Resolve(rd.RegionType));
                    } else
                        if (wrcc.CreationOp == WpfContextControllerCreationOp.ViewModelFirst)
                        {
                            rm.RegisterViewWithRegion(rd.RegionName,
                            () => (regionContext.Container.Resolve(rd.RegionType) as IViewModel).View);

                        }
                }


            }

            IViewContainer vc=context.Container.Resolve<IViewContainer>();

            //code here to handle view model extensions
            bool foundViewModel=AppCanUtils.FindRegisteredType(context.Container, typeof(IViewModel));
            bool foundViewModelExtensions = AppCanUtils.FindRegisteredType(context.Container, typeof(IViewModelExtension));

            if (foundViewModel && foundViewModelExtensions)
            {

                IViewModelExtensionsController vmec = context.ContextDef.Container.Resolve<IViewModelExtensionsController>();
                
                vmec.FindAndAttachViewExtensions(context);
            }

            //For view first we need to show the window
            if (_creationOp == WpfContextControllerCreationOp.ViewFirst)
            {
                Window win = vc as Window;
                if (win != null)
                {
                    win.Show();
                }
            }

            
        }

        /// <summary>
        /// Called when the context is activated (should show any windows/views)
        /// </summary>
        /// <param name="context"></param>
        public virtual void Activate(Core.Contexts.IContext context)
        {
            
        }

        /// <summary>
        /// When called this context will no longer be used after this call returns - note the context could be re-activated at a future time
        /// If the context manager config has lifetime configured for re-use (future functionality)
        /// </summary>
        /// <param name="context"></param>
        public virtual void Release(Core.Contexts.IContext context)
        {
            
        }
    }
}
