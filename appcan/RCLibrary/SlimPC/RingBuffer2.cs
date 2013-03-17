/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading;



namespace RCLibrary.SlimPC
{

    /// <summary>

    /// 

    /// </summary>

    public interface IConsumer
    {

        long StartSequence { get; }

        long EndSequence { get; }



    }



    /// <summary>

    /// 

    /// </summary>

    class Consumer : IConsumer
    {

        private CacheLineStorageLong _startSequence = new CacheLineStorageLong(-1L);

        private CacheLineStorageLong _endSequence = new CacheLineStorageLong(-1L);



        public long StartSequence { get { return _startSequence.Data; } set { _startSequence.Data = value; } }



        public long EndSequence { get { return _endSequence.Data; } set { _endSequence.Data = value; } }



    }





    /// <summary>
    /// Ring based store of reusable entries containing the data representing an <see cref="Entry{T}"/> being exchanged between producers and consumers.
    /// Based only supports one producer thread, and one consumer thread.
    /// </summary>
    /// <typeparam name="T">Entry implementation storing the data for sharing during exchange or parallel coordination of an event.</typeparam>
    public sealed class RingBuffer2<T> where T : class, new()
    {



        private CacheLineStorageLong _cursor = new CacheLineStorageLong(-1L);

        private CacheLineStorageLong _sequence = new CacheLineStorageLong(0);

        private readonly Entry<T>[] _entries;

        private readonly int _ringModMask;

        private CacheLineStorageLong _consumerSequence = new CacheLineStorageLong(0);

        private Consumer _consumer = new Consumer();

        private int _sizeAsPowerOfTwo = 0;

        private long _lastConsumerMinimum = -1;



        /// <summary>

        /// Construct a RingBuffer with the full option set.

        /// </summary>

        /// <param name="entryFactory"> entryFactory to create instances of T for filling the RingBuffer</param>

        /// <param name="size">size of the RingBuffer that will be rounded up to the next power of 2</param>

        /// <param name="claimStrategyOption"> threading strategy for producers claiming entries in the ring.</param>

        /// <param name="waitStrategyOption">waiting strategy employed by consumers waiting on entries becoming available.</param>

        public RingBuffer2(int size)
        {



            _sizeAsPowerOfTwo = Util.CeilingNextPowerOfTwo(size);

            _ringModMask = _sizeAsPowerOfTwo - 1;

            _entries = new Entry<T>[_sizeAsPowerOfTwo];





            Fill();

        }



        public long ProducerGetNextSequence()
        {

            EnsureConsumersAreInRange(_sequence.Data);

            return _sequence.Data++;

        }



        public long ProducerGetNextSequenceBatch(long size)
        {

            long current = _sequence.Data;

            _sequence.Data += size;

            EnsureConsumersAreInRange(current + size);

            return current;



        }



        private void EnsureConsumersAreInRange(long sequence)
        {

            var wrapPoint = sequence - _sizeAsPowerOfTwo;



            while (wrapPoint > (_lastConsumerMinimum = _consumer.StartSequence))
            {

                Thread.Yield();

            }

        }



        public long ConsumerGetNextSequence(out long size)
        {

            long currentConsumer = _consumerSequence.Data;

            long currentCursor = _cursor.Data;

            size = currentCursor - currentConsumer;

            if (size <= 0)
                size = 1;

            _consumerSequence.Data += size;



            ConsumerWaitFor(currentConsumer + size - 1);



            _consumer.StartSequence = currentConsumer;

            _consumer.EndSequence = currentConsumer + size;


            //System.Diagnostics.Debug.Assert(currentConsumer != -1);
            return currentConsumer;

        }





        public long ConsumerWaitFor(long sequence)
        {





            while (sequence > _cursor.Data)

                Thread.Yield();



            return sequence;

        }











        public IConsumer GetConsumer()
        {

            return _consumer;



        }







        public void Commit(long sequence)
        {



            Cursor = sequence; // volatile write



        }





        /// <summary>

        /// The capacity of the RingBuffer to hold entries.

        /// </summary>

        public int Capacity
        {

            get { return _entries.Length; }

        }



        /// <summary>

        /// Get the current sequence that producers have committed to the RingBuffer.

        /// </summary>

        public long Cursor
        {

            get { return _cursor.Data; }

            private set { _cursor.Data = value; }

        }



        ///<summary>

        /// Get the <see cref="Entry{T}"/> for a given sequence in the RingBuffer.

        ///</summary>

        ///<param name="sequence">sequence for the <see cref="Entry{T}"/></param>

        public Entry<T> this[long sequence]
        {

            get
            {

                return _entries[(int)sequence & _ringModMask];

            }

        }

















        ///<summary>

        /// Calls <see cref="IBatchConsumer.Halt"/> on all the consumers

        ///</summary>

        public void Halt()
        {



        }









        private void Fill()
        {

            for (var i = 0; i < _entries.Length; i++)
            {

                var data = new T();

                _entries[i] = new Entry<T>(-1, data);

            }

        }

    }

}

