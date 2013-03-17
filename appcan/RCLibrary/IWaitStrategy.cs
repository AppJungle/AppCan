/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCLibrary
{
    public interface IWaitStrategy
    {
        /// <summary>
        /// Called when no items currently in the pool, will wait for items to be returned.
        /// </summary>
        /// 
        /// <returns>true if wait timed out, false if objects should be in pool</returns>
          bool Wait();


        /// <summary>
        /// Called when there are items in the pool to indicate any threads can continue
        /// </summary>
          void Set();
        

        /// <summary>
        /// Called when there are NO items in the pool
        /// </summary>
          void Reset();
        

    }
}
