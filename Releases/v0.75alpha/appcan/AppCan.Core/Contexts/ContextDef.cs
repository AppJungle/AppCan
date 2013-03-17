/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Threading;
using AppCan.Core.Contexts;
using AppCan.Core.Services;

namespace AppCan.Core.Contexts
{
    /// <summary>
    /// A context definition,defines a context it contains registered types in it's container
    /// </summary>
    public class ContextDef : AppCan.Core.Contexts.IContextDef
    {
        //List<Context> _contexts = new List<Context>();
        
        /// <summary>
        /// _contexts access keeps the list of active contexts, must be protected by a lock since may be accessed from
        /// the UI thread and the finalizer threads
        /// </summary>
        Dictionary<int, WeakReference> _contexts = new Dictionary<int, WeakReference>();
        IContextManager _contextManager;
        string _name;
        
        IUnityContainer _rootContainer;
        IUnityContainer _container = new UnityContainer();
        IContextController _contextController;
        IServiceReplicator _serviceReplicator;

        private int _contextInstanceID = 0;

        /// <summary>
        /// Contains the list key pairs of the instance ID and weak references to context
        /// </summary>
        public List<KeyValuePair<int, WeakReference>> Contexts
        {
            get
            {
                lock (_contexts)
                {
                    return _contexts.ToList();
                }
            }
        }


        internal ContextDef(IContextManager contextManager,string name, IUnityContainer rootContainer, IContextController contextController, IServiceReplicator serviceReplicator )
        {
            _contextManager = contextManager;
            _name = name;
            _rootContainer = rootContainer;
            _contextController = contextController;
            if (_contextController != null)
            {
                _contextController.ContextDefInit(this);
                
                _container.RegisterInstance<IContextController>(_contextController);

            }

            _serviceReplicator = serviceReplicator;

        }

        
        
        
        /// <summary>
        /// The container associated with this context definition.  Types registered to this container will be available to create in
        /// any of the created contexts
        /// </summary>
        public IUnityContainer Container { get { return _container; } }


        /// <summary>
        /// Create/Get a new context
        /// </summary>
        /// <returns></returns>
        public virtual Context GetNewContext()
        {
            
            return GetNewContext(null);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Create a context and pass the parent context or Null
        /// </summary>
        /// <param name="parentContext"></param>
        /// <returns></returns>
        public virtual Context GetNewContext(IContext parentContext)
        {
            int nextid = Interlocked.Increment(ref _contextInstanceID);
            Context ctx = new Context(this, _container.CreateChildContainer(), _rootContainer, _contextController, nextid,parentContext);

            lock (_contexts)
            {
                _contexts.Add(ctx.InstanceID, new WeakReference(ctx, false));
            }

            //Replicate services to the container prior to Init, so if Init needs access to services it will get them
            _serviceReplicator.ReplicateToContainer(ctx.Container);

            ctx.Init();
            return ctx;
            //throw new NotImplementedException();
        }

        internal protected virtual bool ReleaseReference(int instanceID)
        {
            lock (_contexts)
            {
                return _contexts.Remove(instanceID);
            }
        }

    }
}
