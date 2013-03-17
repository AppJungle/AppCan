/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace RCLibrary
{
    /// <summary>
    /// This class is a thread safe implementation of StackPool
    /// It internally uses a spin lock
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StackPoolEx<T> : StackPoolExBase<T> where T : new()
    {
        #region Private Variables
        const uint DEFAULT_SIZE = 25;

        
        ILockStrategy _lock = new SpinLockStrategy();

        IWaitStrategy _waitStrategy = new EventWaitStrategy();

        
        int _waitCount = 0;

        #endregion Private Variables

        #region Properties
        /// <summary>
        /// Diagnostic count to determine how many times the pool needed to wait for more objects.
        /// For some uses where no waits are desired the default pool size should be increased
        /// </summary>
        public int WaitCount { get { return _waitCount; } }
        #endregion Properties

        #region Constructors
        public StackPoolEx()
            : base(DEFAULT_SIZE, true, true)
        {

        }

        public StackPoolEx(uint capacity)
            : base(capacity, false, true)
        {



        }

        public StackPoolEx(uint capacity, bool disposeOnRelease)
            : base(capacity, disposeOnRelease, true)
        {



        }

        public StackPoolEx(uint capacity, bool disposeOnRelease, bool poolStamping)
            : base(capacity, disposeOnRelease, poolStamping)
        {



        }

        public StackPoolEx(uint capacity, bool disposeOnRelease, bool poolStamping, bool waitForObjects)
            : base(capacity, disposeOnRelease, poolStamping)
        {

            WaitForObjects = waitForObjects;

        }

        public StackPoolEx(uint capacity, bool disposeOnRelease, bool poolStamping, bool waitForObjects, ILockStrategy lockStrategy)
            : this(capacity, disposeOnRelease, poolStamping,waitForObjects)
        {

            _lock = lockStrategy;

        }

        public StackPoolEx(uint capacity, bool disposeOnRelease, bool poolStamping, bool waitForObjects, IWaitStrategy waitStrategy)
            : this(capacity, disposeOnRelease, poolStamping, waitForObjects)
        {

            _waitStrategy = waitStrategy;

        }

        public StackPoolEx(uint capacity, bool disposeOnRelease, bool poolStamping, bool waitForObjects, ILockStrategy lockStrategy, IWaitStrategy waitStrategy)
            : this(capacity, disposeOnRelease, poolStamping, waitForObjects)
        {

            _lock = lockStrategy;
            _waitStrategy = waitStrategy;
            
        }

        #endregion Constructors

        #region virtuals

        

        protected override void Lock(ref bool val)
        {
            _lock.Lock(ref val);

        }

        protected override void Exit()
        {
            _lock.Exit();

        }


        #region Wait for objects virtual methods
        /// <summary>
        /// Called when no items currently in the pool, will wait for items to be returned.
        /// </summary>
        protected override bool Wait()
        {
            _waitCount++;
            _waitStrategy.Wait();
            return false;
        }

        /// <summary>
        /// Called when there are items in the pool to indicate any threads can continue
        /// </summary>
        protected override void Set()
        {
            _waitStrategy.Set();

        }

        /// <summary>
        /// Called when there are NO items in the pool
        /// </summary>
        protected override void Reset()
        {

            _waitStrategy.Reset();

        }
        #endregion Wait for objects virtual methods

        #endregion virtual
    }
}
