/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/


using System.Diagnostics;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCLibrary.SlimPC;
using System.Threading;

namespace TestProject1
{
    class MyValue
    {

        public long Sequence;



    }


    [TestClass]
    public class RingBufferTest
    {
        const long BATCH_SIZE = 1000;

        const long MAX_ITEMS = 100000;

        static RingBuffer2<MyValue> buffer = new RingBuffer2<MyValue>(1024 * 32);


        [TestMethod]
        public void RingBufferTest1()
        {

            Thread oThread = new Thread(new ThreadStart(RingBufferTest.MyThread));



            // Start the thread

            oThread.Start();







            long count = 0;



            var sw = Stopwatch.StartNew();

            while (count < MAX_ITEMS)
            {

                long current = buffer.ProducerGetNextSequenceBatch(BATCH_SIZE);

                long end = current + BATCH_SIZE;



                for (long i = current; i < end; i++)
                {

                    buffer[i].Data.Sequence = count;



                    //buffer.Commit(i);

                    count++;

                }



                buffer.Commit(count - 1);





            }



            long milliseconds = sw.ElapsedMilliseconds;

            Console.WriteLine("Producer Milliseconds:{0}", milliseconds);
        }


        static void MyThread()
        {

            long sequence = -1;

            long count = 0;

            long size;

            var sw = Stopwatch.StartNew();



            while (count < MAX_ITEMS - 1)
            {



                sequence = buffer.ConsumerGetNextSequence(out size);

                long end = sequence + size;

                for (long i = sequence; i < end; i++)
                {

                    long val = buffer[i].Data.Sequence;

                    if (val != i)

                        throw new Exception("Invalid Value");

                    count++;

                }



            }



            long milliseconds = sw.ElapsedMilliseconds;

            Console.WriteLine("Consumer Milliseconds:{0}", milliseconds);


        }
    }
}
