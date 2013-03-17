/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using AppCan.Core.Contexts;
using AppCan.Core.Services;

namespace AppCan.Core.Contexts
{
    /// <summary>
    /// Contexts are an instance of type/instances that were added to a context definition and created when a new context 
    /// is created.  Contexts hold types to be resolved to instances, as well as a controller that assists with the lifetime
    /// for the context.  
    /// </summary>
    public class Context : IContext 
    {
        private IUnityContainer _container;
        private IUnityContainer _rootContainer;
        private ContextDef _contextDef;
        private IContextController _contextController;
        private IContext _parentContext=null;
        

        private int _instanceID;

        internal Context(ContextDef contextdef,IUnityContainer container,IUnityContainer rootContainer,IContextController contextController, int instanceID)
        {
            

            _contextDef = contextdef;
            _container = container;
            _rootContainer = rootContainer;
            _contextController = contextController;
            _instanceID = instanceID;

            //register this instance into our container so it can be autopopulated from any type that needs it.
            _container.RegisterInstance<IContext>(this);

            if (_contextController!=null)
                _container.RegisterInstance<IContextController>(_contextController);

        }

        internal Context(ContextDef contextdef, IUnityContainer container, IUnityContainer rootContainer, IContextController contextController, int instanceID, IContext parentContext) : this(contextdef,container,rootContainer,contextController,instanceID)
        {
            _parentContext = parentContext;
        

        }

        /// <summary>
        /// The Container associated with this context
        /// </summary>
        public IUnityContainer Container { get { return _container; } }

        /// <summary>
        /// The root container
        /// </summary>
        public IUnityContainer RootContainer { get { return _rootContainer; } }


        /// <summary>
        /// The parent context or null
        /// </summary>
        public IContext ParentContext { get { return _parentContext; } }

        /// <summary>
        /// The instance ID of this context
        /// </summary>
        public int InstanceID { get { return _instanceID; } }

        /// <summary>
        /// Called when the context is first created
        /// </summary>
        public void Init()
        {
            if (_contextController != null)
                _contextController.Init(this);

        }

        /// <summary>
        /// Called when the context is going to create views/show windows
        /// </summary>
        public void Create()
        {
            if (_contextController != null)
                _contextController.Create(this);
        }

        /// <summary>
        /// Activates existing windows/views within the context
        /// </summary>
        public void Activate()
        {
            if (_contextController != null)
                _contextController.Activate(this);
        }

        /// <summary>
        /// Calling this means your objects external to this context have released all reference/use or need of it.
        /// Do not use the object after calling this.  The object may be recycled by the context definition so use after releasing 
        /// would cause a problem.
        /// </summary>
        public void Release()
        {
            if (_contextController != null)
                _contextController.Release(this);

            ReleaseReference();

            //TODO: Consider suppressing finalize here IF we don't plan to recycle this object.
        }

        /// <summary>
        /// Called when the context is no longer needed (i.e. IViewContainer closed, etc.) 
        /// </summary>
        protected void ReleaseReference()
        {
            if (_contextDef != null)
            {
                _contextDef.ReleaseReference(this.InstanceID);
            }
        }

        /// <summary>
        /// The reference to the context definition that created this context
        /// </summary>
        public IContextDef ContextDef
        {
            get { return _contextDef; }
        }

        /// <summary>
        /// Finalizer to remove the reference in the context def to this context
        /// </summary>
       ~Context()
        {
            ReleaseReference();
            

        }
    }
}
