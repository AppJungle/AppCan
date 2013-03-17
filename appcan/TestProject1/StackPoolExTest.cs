/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using RCLibrary;
using System.Diagnostics;

namespace TestProject1
{
    /// <summary>
    /// Summary description for StackPoolExTest
    /// </summary>
    [TestClass]
    public class StackPoolExTest
    {
        public StackPoolExTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        public class TestItem
        {
            public int item;

        }

        uint _numberOfItems = 0;
        

        [TestMethod]
        public void StackPoolExTestWait()
        {
            _numberOfItems = 25;

            StackPoolEx<TestItem> pool = new StackPoolEx<TestItem>(_numberOfItems, false, false, true);

            Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod));

            TestItem item = pool.Get();
            item.item = 1;

            t.Start(pool);

            bool terminated=t.Join(5000);
            if (terminated)
            {
                Assert.Fail("Thread terminated and did not wait for an object");
                return;
            }

            pool.Release(item);

            terminated = t.Join(5000);
            if (!terminated)
            {
                Assert.Fail("Thread did not terminate as expected waiting for the object");
                return;
            }

            Assert.AreEqual(terminated, true);


        }

        void ThreadMethod(object parameter)
        {
            StackPoolEx<TestItem> pool = parameter as StackPoolEx<TestItem>;

            for (int x = 0; x < _numberOfItems; x++)
            {
                TestItem item = pool.Get();
            }

            
        }

        //[TestMethod]
        public void StackPoolExTestTime()
        {

            const int MAX_ITEMS = 30000000;

            List<TestItem> items = new List<TestItem>(MAX_ITEMS);

            //StackPoolEx<TestItem> pool = new StackPoolEx<TestItem>(MAX_ITEMS, false, false, true);
            StackPoolSlim<TestItem> pool = new StackPoolSlim<TestItem>(MAX_ITEMS, false);
            GC.Collect();
            GC.Collect();


            int start=Environment.TickCount;

            for (int x = 0; x < MAX_ITEMS; x++)
            {
                items.Add(pool.Get());
            }

            int end = Environment.TickCount;

            //Trace.WriteLine("Difference:"+ (end - start).ToString());
            //Console.WriteLine("{0}",end-start);

            Assert.IsTrue(false, "Difference:" + (end - start).ToString());

        }

    }
}
