using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Diagnostics;

using System.Threading;



namespace ConsoleApplication2
{

    class MyValue
    {

        public long Sequence;



    }



    class Program
    {

        const long BATCH_SIZE = 1000;

        const long MAX_ITEMS = 100000000;

        static RingBuffer2<MyValue> buffer = new RingBuffer2<MyValue>(1024 * 32);



        static void Main(string[] args)
        {





            // Create the thread object, passing in the Alpha.Beta method

            // via a ThreadStart delegate. This does not start the thread.

            Thread oThread = new Thread(new ThreadStart(Program.MyThread));



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







