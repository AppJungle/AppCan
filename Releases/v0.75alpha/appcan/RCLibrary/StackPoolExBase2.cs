/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Concurrent;

namespace RCLibrary
{
    /// <summary>
    /// Stack pool - future improvement ideas... 
    /// reference count to catch double release
    /// finalizable object support with internal release to totally avoid double release
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StackPoolExBase2<T> : IItemPool<T> where T : new()
    {
        #region Private Variables
        //LinkedList<T> _stack=null;
        const int DEFAULT_STACK_ALLOC = 25;

        ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        
        volatile LNode _freeHead = null;
        volatile LNode _listHead = null;
        
        bool _disposeOnRelease = false;
        bool _poolStamping = true;
        bool _waitForObjects = false;

        volatile int _freeNodeCount = 0;
        volatile int _allocatedItems = 0;
        volatile int _objectsInPool = 0;

        


        #endregion Private Variables


        #region Properties
        public int AllocatedObjects
        {
            get
            {
                return _allocatedItems;
            }
        }
        #endregion Properties

        #region Protected Properties
        protected bool WaitForObjects
        {
            get { return _waitForObjects; }
            set { _waitForObjects = true; }

        }
        #endregion Protected Properties


        #region Constructors
        public StackPoolExBase2()
            : this(25, true, true)
        {

        }

        public StackPoolExBase2(uint capacity)
            : this(capacity, false, true)
        {



        }

        public StackPoolExBase2(uint capacity, bool disposeOnRelease)
            : this(capacity, disposeOnRelease, true)
        {



        }



        public StackPoolExBase2(uint capacity, bool disposeOnRelease, bool poolStamping)
        {
            _disposeOnRelease = disposeOnRelease;
            _poolStamping = poolStamping;
            Alloc(capacity);


        }
        #endregion Constructors

        public T Get()
        {
            T value = default(T);
            bool gotLock = false;

            try
            {


                Lock(ref gotLock);

                if (_listHead == null && !_waitForObjects)
                {
                    Alloc(DEFAULT_STACK_ALLOC);

                }
                else
                    if (_waitForObjects)
                    {
                        while (_listHead == null)
                        {
                            gotLock = false;
                            Exit();
                            bool timeOut = Wait();
                            Lock(ref gotLock);

                            //check if the derived class requested to timeout allocate objects
                            if (timeOut && _listHead == null)
                            {
                                //we timed out - allocate objects.
                                Alloc(DEFAULT_STACK_ALLOC); //this could cause a GC if ran out of space to allocate from
                                break;
                            }
                        }

                    }

                LNode node = _listHead;
                _listHead = node.Next;

                value = node.Value;
                FreeNode(node);

                _objectsInPool--;

                if (_objectsInPool == 0)
                    Reset();
            }
            finally
            {
                if (gotLock)
                    Exit();

            }

            return value;
        }

        /// <summary>
        /// Internal release object method
        /// </summary>
        /// <param name="item">The item being returned to the pool</param>
        /// <param name="noDispose">true to not call dispose on the T class, false will call dispose if the object supports it</param>
        private void Release(T item, bool noDispose)
        {
            //if disposing on release is enabled dispose if it's disposable
            if (!noDispose && _disposeOnRelease && (item is IDisposable))
            {
                IDisposable iItem = item as IDisposable;
                iItem.Dispose();
            }

            bool gotLock = false;
            try
            {
                Lock(ref gotLock);

                if (_listHead == null)
                {
                    _listHead = GetNode(item);
                    _listHead.Next = null;
                }
                else
                {
                    LNode node = GetNode(item);
                    node.Next = _listHead;
                    _listHead = node;
                }

                _objectsInPool++;

                if (_objectsInPool == 1)
                    Set();
            }
            finally
            {
                if (gotLock)
                    Exit();

            }
        }

        public void Release(T item)
        {
            Release(item, false);
        }


        /// <summary>
        /// If objects contain native handles be sure the objects have finalizers to handle that case before this is used.
        /// This just clears the entire pool.  It does not call dispose or anything else.  Clearing will make the objects
        /// that are left in the pool, not reachable so they will be garbage collected at a future point.
        /// It is possible after this is called that there are items still in use and they will be released back to the pool
        /// by the user code when it is done with the object. (So even after clearing you might discover objects back in the pool)
        /// This also clears all internal datastructures that were being pooled to store objects in the pool and objects it expects to return to the pool.
        /// Also requests to get new objects will allocate them.
        /// </summary>
        public void Clear()
        {

            bool gotLock = false;

            try
            {


                Lock(ref gotLock);

                //clear free object list and associated nodes GC will happen!
                _listHead = null;

                //clear free node list - GC will happen!
                _freeHead = null;

                //clear the counts
                _allocatedItems = 0;
                _freeNodeCount = 0;
                _objectsInPool = 0;
            }
            finally
            {
                if (gotLock)
                    Exit();

            }

        }

        #region Private Methods
        /// <summary>
        /// No locking, outter method needs to lock
        /// </summary>
        /// <param name="size"></param>
        private void Alloc(uint size)
        {
            for (int x = 0; x < size; x++)
            {
                T newItem = new T();
                if (_poolStamping)
                {
                    IUsesItemPool<T> iUsePool = newItem as IUsesItemPool<T>;
                    if (iUsePool != null)
                        iUsePool.ItemPool = this as IItemPool<T>;
                }

                Release(newItem, true);
                _allocatedItems++;

            }
        }

        /// <summary>
        /// No locking, outter method needs to lock
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private LNode GetNode(T value)
        {
            if (_freeHead == null)
                return new LNode(value);

            LNode node = _freeHead;
            _freeHead = node.Next;

            node.Value = value;

            //Keep count of free'd nodes
            _freeNodeCount--;

            return node;
        }


        /// <summary>
        /// No locking, outter methods need to lock
        /// </summary>
        /// <param name="node"></param>
        private void FreeNode(LNode node)
        {
            node.Value = default(T);
            node.Next = _freeHead;
            _freeHead = node;

            //Keep count of free'd nodes
            _freeNodeCount++;
            return;
        }
        #endregion Private Methods

        #region virtual methods

        /// <summary>
        /// Virtual calls for locking
        /// </summary>
        /// <param name="gotLock">Is set to true if lock was taken, otherwise exception is thrown</param>
        protected virtual void Lock(ref bool gotLock)
        {


        }

        /// <summary>
        /// Exit lock - derived classes use for locking
        /// </summary>
        protected virtual void Exit()
        {


        }

        #region Wait for objects virtual methods




        /// <summary>
        /// Called when no items currently in the pool, will wait for items to be returned.
        /// </summary>
        /// 
        /// <returns>true if wait timed out, false if objects should be in pool</returns>
        protected virtual bool Wait()
        {

            return false;
        }

        /// <summary>
        /// Called when there are items in the pool to indicate any threads can continue
        /// </summary>
        protected virtual void Set()
        {


        }

        /// <summary>
        /// Called when there are NO items in the pool
        /// </summary>
        protected virtual void Reset()
        {



        }
        #endregion Wait for objects virtual methods



        #endregion virtual methods


        #region LNode class
        protected class LNode
        {
            public T Value;
            public LNode Next;

            public LNode(T value)
            {
                Value = value;
                Next = null;
            }

            /*~LNode()
            {
                int x = 0;
                x++;

            }*/


        }
        #endregion LNode class
    }

}
