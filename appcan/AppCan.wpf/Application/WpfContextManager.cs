/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.Core.Contexts;
using Microsoft.Practices.Unity;
using System.Xml;
using AppCan.wpf.Regions;
using AppCan.wpf.Contexts;
using AppCan.wpf.Application;

namespace AppCan.wpf.Application
{
    /// <summary>
    /// The wpf context manager.  Derived from the context manager adds functionality specific to wpf windows
    /// </summary>
    public class WpfContextManager : ContextManager, IWpfContextManager
    {
        private const string S_CONTEXTDEF = "contextdef";
        private const string S_NAME = "name";
        private const string S_REGIONS = "regions";
        private const string S_REGION = "region";
        private const string S_TYPE = "type";
        private const string S_CONTEXT = "context";
        private const string S_CREATEOP = "createop";

        /// <summary>
        /// DEfault constructor
        /// </summary>
        /// <param name="rootContainer">The root container</param>
        public WpfContextManager(IUnityContainer rootContainer) : base(rootContainer)
        {
            //_rootContainer = rootContainer;

        }

        /// <summary>
        /// Constructor with the root container and the controller type passed in.
        /// </summary>
        /// <param name="rootContainer">The root container</param>
        /// <param name="contextControllerType">The type of the context controller to use.  Type will be resolved from the root container for each context definition</param>
        public WpfContextManager(IUnityContainer rootContainer, Type contextControllerType)
            : base(rootContainer, contextControllerType)
        {
            //_rootContainer = rootContainer;
            //_defaultContextController = contextController;

        }

        /// <summary>
        /// Get or create a contextdefinition and set it's creation operation.
        /// </summary>
        /// <param name="contextName"></param>
        /// <param name="creationOp"></param>
        /// <returns></returns>
        public virtual IContextDef GetNewOrExistingContextDef(string contextName,WpfContextControllerCreationOp creationOp)
        {
            IContextDef cd=GetNewOrExistingContextDef(contextName);

            IWpfContextController wcc = cd.Container.Resolve<IContextController>() as IWpfContextController;
            wcc.CreationOp = creationOp;

            return cd;
        }


         /// <summary>
         /// Called after ContextManager specific configuration was loaded
         /// </summary>
         /// <param name="doc"></param>
         protected override void LoadConfigurationAfter(XmlDocument doc)
         {
             XmlNodeList contextdefs = doc.SelectNodes("//"+S_CONTEXTDEF);  //TODO:Cleanup string allocation
             foreach (XmlNode contextdef in contextdefs)
             {
                 string name = contextdef.Attributes[S_NAME].Value;
                 //Console.WriteLine("ContextDef name={0}", contextdef.Attributes[S_NAME].Value);
                 
                 //Parse the createOp for wpf configuration
                 bool foundCreateOp = false;
                 WpfContextControllerCreationOp createOpEnum = WpfContextControllerCreationOp.ViewFirst;
                 if (contextdef.Attributes[S_CREATEOP] != null)
                 {
                     string createOpString = contextdef.Attributes[S_CREATEOP].Value;
                     try
                     {
                         createOpEnum = (WpfContextControllerCreationOp)Enum.Parse(typeof(WpfContextControllerCreationOp), createOpString);
                         if (Enum.IsDefined(typeof(WpfContextControllerCreationOp), createOpEnum)) // | createOpEnum.ToString().Contains(","))
                             foundCreateOp = true;
                         else
                         {
                             //TODO: Log bad value
                         }
                             
                     }
                     catch (ArgumentException)
                     {
                         //TODO: Log bad value
                     }

                 }

                 IContextDef currentContextDef;

                 if (foundCreateOp)
                    currentContextDef = GetNewOrExistingContextDef(name,createOpEnum);
                 else
                    currentContextDef = GetNewOrExistingContextDef(name);
                 //End parse createOp for Wpf viewmodelfirst or viewfirst configuration

                 IContextRegionManager contextRegionManager=currentContextDef.Container.Resolve<IContextRegionManager>();
                 XmlNodeList regions = contextdef.SelectNodes(S_REGIONS+"/"+S_REGION);

                 //TODO: Associate a region with a context, make the context region manager delegate use the associated contexts container
                 
                 foreach (XmlNode region in regions)
                 {
                     string regionName = region.Attributes[S_NAME].Value;
                     string regionTypeString = region.Attributes[S_TYPE].Value;
                     string regionContext=null;
                     if (region.Attributes[S_CONTEXT] != null)
                         regionContext = region.Attributes[S_CONTEXT].Value;

                     Type regionType = Type.GetType(regionTypeString); //Type.GetType("Namespace.MyClass, MyAssembly");
                     if (regionContext==null)
                        contextRegionManager.RegisterViewWithRegion(regionName, regionType);
                     else
                         contextRegionManager.RegisterViewWithRegion(regionName, regionType,regionContext);
                 }


             }

         }
    }
}
