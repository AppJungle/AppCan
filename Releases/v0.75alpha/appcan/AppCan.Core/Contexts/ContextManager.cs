/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Xml;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using AppCan.Core.Services;


namespace AppCan.Core.Contexts
{
    /// <summary>
    /// This is the context manager class it allows the creating of context definitions that contain containers for registering types.
    /// Context's can be created from contextdefs
    /// </summary>
    public class ContextManager : AppCan.Core.Contexts.IContextManager
    {
        private Dictionary<string, ContextDef> _contextDefs=new Dictionary<string,ContextDef>();
        IUnityContainer _rootContainer;
        Type _defaultContextControllerType;
        private IServiceReplicator _serviceReplicator;

        /// <summary>
        /// Constructor for the context manager
        /// </summary>
        /// <param name="rootContainer">The root container</param>
        public ContextManager(IUnityContainer rootContainer)
        {
            _rootContainer = rootContainer;
            
            // Create the service replicator and register it with the root container
            _serviceReplicator = new ServiceReplicator(rootContainer,true);

            rootContainer.RegisterInstance<IServiceReplicator>(_serviceReplicator);
            

        }

        /// <summary>
        /// Constructor with the root container and the controller type
        /// </summary>
        /// <param name="rootContainer">The root container</param>
        /// <param name="contextControllerType">The controller type</param>
        public ContextManager(IUnityContainer rootContainer,Type contextControllerType) : this(rootContainer)
        {
            
            _defaultContextControllerType = contextControllerType;

        }

        /// <summary>
        /// Not implemented yet.
        /// </summary>
        /// <param name="contextName"></param>
        /// <returns></returns>
        public Context GetContext(string contextName)
        {
            //find a context def if it exists, otherwise return null
            //if there is a context def call GetContext on it.
            //The context def will return either a new context or a re-used context.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get's a new or existing context.  The context definition must alreay exist.
        /// </summary>
        /// <param name="contextName">The context name</param>
        /// <returns>, if contextdef does not exist, null is returned</returns>
        public Context GetNewOrExistingContext(string contextName)
        {
            ContextDef cdef; //= _contextDefs[contextName];
            bool hasValue = _contextDefs.TryGetValue(contextName, out cdef);
            if (hasValue)
            {

                return cdef.GetNewContext();
                
            }

            return null;
        }

        /// <summary>
        /// Gets a new or existing Context definition
        /// </summary>
        /// <param name="contextName">The context name</param>
        /// <returns>The context definition</returns>
        public virtual ContextDef GetNewOrExistingContextDef(string contextName)
        {
            ContextDef cdef; //= _contextDefs[contextName];
            bool hasValue=_contextDefs.TryGetValue(contextName, out cdef);
            if (!hasValue)
            {
                IContextController cc=_rootContainer.Resolve(_defaultContextControllerType) as IContextController;
                cdef = new ContextDef(this,contextName, _rootContainer,cc,_serviceReplicator);
                _contextDefs[contextName] = cdef;
            }

            return cdef;

        }


        const string S_UNITYCONFIGFILE = "unityconfigfile";
        const string S_CONTEXTDEFS = "contextdefs";
        const string S_CONTEXTDEF = "contextdef";
        const string S_NAME = "name";
        const string S_ALIAS = "alias";
        const string S_TYPE = "type";
        const string S_LIFETIME = "lifetime";
        const string S_INSTANCE = "instance";


        /// <summary>
        /// Load the contextdef's from a xml configuration file 
        /// </summary>
        /// <param name="fileName"></param>
        public virtual void LoadConfiguration(string fileName)
        {
            string unityConfigFile = null;

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            //interception point for derived class
            LoadConfigurationBefore(doc);
            
            XmlNode cdfs=doc.SelectSingleNode("//"+S_CONTEXTDEFS);  //TODO: Fix append to not create extra string

            if (cdfs == null)
            {
                throw new InvalidOperationException("Missing xml:" + S_CONTEXTDEFS + " in file:" + fileName);
            }

            ExeConfigurationFileMap map=null;

            System.Configuration.Configuration config=null;
              
            

            bool useUnityConfig = false;
            if (cdfs.Attributes[S_UNITYCONFIGFILE] != null)
            {
                unityConfigFile = cdfs.Attributes[S_UNITYCONFIGFILE].Value;
                 map= new ExeConfigurationFileMap();
                 map.ExeConfigFilename = unityConfigFile;
                config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                useUnityConfig = true;
            }


            XmlNodeList contextdefs = doc.SelectNodes("//" + S_CONTEXTDEF); //TODO: Fix append to not create extra string
            foreach (XmlNode contextdef in contextdefs)
            {
                string name = contextdef.Attributes[S_NAME].Value;
                //Console.WriteLine("ContextDef name={0}", contextdef.Attributes["name"].Value);

                ContextDef currentContextDef=GetNewOrExistingContextDef(name);

                
                XmlNodeList registerTypes = contextdef.SelectNodes("registertypes/registertype");
                foreach (XmlNode registerType in registerTypes)
                {
                    string aliasName=null;
                    if (registerType.Attributes[S_ALIAS]!=null)
                        aliasName = registerType.Attributes[S_ALIAS].Value;
                    
                    string typeName=null;
                    if (registerType.Attributes[S_TYPE] != null)
                        typeName = registerType.Attributes[S_TYPE].Value;

                    string lifeTime=null;
                    if (registerType.Attributes[S_LIFETIME]!=null)
                        lifeTime = registerType.Attributes[S_LIFETIME].Value;

                    string registerName = null;
                    if (registerType.Attributes[S_NAME] != null)
                        registerName = registerType.Attributes[S_NAME].Value;

                    Type aliasType=null;
                    if (aliasName!=null)
                        aliasType = Type.GetType(aliasName); //Type.GetType("Namespace.MyClass, MyAssembly");

                    Type typeType=null;
                    if (typeName!=null)
                        typeType = Type.GetType(typeName);

                    if (lifeTime == null || lifeTime.ToLower() != S_INSTANCE)
                    {
                        if (aliasType != null)
                        {
                            if (registerName != null)
                                currentContextDef.Container.RegisterType(aliasType, typeType, registerName, new ContainerControlledLifetimeManager());
                            else
                                currentContextDef.Container.RegisterType(aliasType, typeType, new ContainerControlledLifetimeManager()); // a single instance will be created
                        }
                        else
                        {
                            if (registerName != null)
                                currentContextDef.Container.RegisterType(typeType, registerName,new ContainerControlledLifetimeManager()); // a single instance will be created
                            else
                                currentContextDef.Container.RegisterType(typeType, new ContainerControlledLifetimeManager()); // a single instance will be created
                        }
                    }
                    else
                    {
                        if (aliasType != null)
                        {
                            if (registerName != null)
                                currentContextDef.Container.RegisterType(aliasType, typeType,registerName);  //an instance per resolve will be created
                            else
                                currentContextDef.Container.RegisterType(aliasType, typeType);  //an instance per resolve will be created
                        }
                        else
                        {
                            if (registerName != null)
                                currentContextDef.Container.RegisterType(typeType,registerName);  //an instance per resolve will be created
                            else
                                currentContextDef.Container.RegisterType(typeType);  //an instance per resolve will be created
                        }
                    }

                    if (useUnityConfig)
                    {
                        
                        UnityConfigurationSection unitySection = (UnityConfigurationSection)config.GetSection(name);
                        if (unitySection!=null)
                            unitySection.Configure(currentContextDef.Container);
                        
                    }

                }

                LoadContextAfter(doc, currentContextDef);

                
            }

            LoadConfigurationAfter(doc);


        }

        /// <summary>
        /// Called after an individual context was loaded - internal call made during a LoadConfiguration call
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="currentContextDef"></param>
        protected virtual void LoadContextAfter(XmlDocument doc, ContextDef currentContextDef)
        {
            
        }

        /// <summary>
        /// Called after ContextManager specific configuration was loaded - internal call made during a LoadConfiguration call
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void LoadConfigurationAfter(XmlDocument doc)
        {
            
        }

        /// <summary>
        /// Called before context manager specific configuration is loaded  - internal call made during a LoadConfiguration call
        /// </summary>
        /// <param name="doc"></param>
        protected virtual void LoadConfigurationBefore(XmlDocument doc)
        {
            
        }
    }
}
