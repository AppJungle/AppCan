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
    public class SpinLockStrategy : ILockStrategy
    {
        SpinLock _lock = new SpinLock();

        /// <summary>
        /// Virtual calls for locking
        /// </summary>
        /// <param name="gotLock">Is set to true if lock was taken, otherwise exception is thrown</param>
        public void Lock(ref bool gotLock)
        {
            _lock.Enter(ref gotLock);

        }

        /// <summary>
        /// Exit lock - derived classes use for locking
        /// </summary>
        public void Exit()
        {
            _lock.Exit();

        }

    }
}
