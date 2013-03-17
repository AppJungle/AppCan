/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCLibrary.SlimPC
{
    /// <summary>
    /// Entries are the items exchanged via a RingBuffer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Entry<T>
    {
        /// <summary>
        /// get: Get the sequence number assigned to this item in the series.
        /// set: Explicitly set the sequence number for this Entry and a CommitCallback for indicating when the producer is
        ///      finished with assigning data for exchange.
        /// </summary>
        public long Sequence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Initialise a new instance of <see cref="Entry{T}"/> with a sequence number and the underlying data
        /// </summary>
        /// <param name="sequence">sequence number.</param>
        /// <param name="data">underlying data.</param>
        public Entry(long sequence, T data)
            : this()
        {
            Sequence = sequence;
            Data = data;
        }
    }
}
