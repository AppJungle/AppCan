/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TaskMaster;
using RCLibrary;
using System.Threading;

namespace TestProject1
{
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This test checks if list order is maintained and if any GC's occur
        /// while adding and removing items.  Uses RCLinkedList constructor,
        /// AddFirst, First and RemoveFirst. 
        /// </summary>
        [TestMethod]
        public void TestRCLinkedList1Basic()
        {
            const int NUMBER_OF_ITEMS = 1000000;

            RCLinkedList<int> list = new RCLinkedList<int>(NUMBER_OF_ITEMS);

            //Collect so we have a clean start to test with
            //Make sure any objects that need to be collected
            //are collected before we start the test.
            //Two collections to ensure even finalizable objects are cleaned up.
            GC.Collect();
            GC.Collect();

            //get the current collection count
            int start0 = GC.CollectionCount(0);
            int start1 = GC.CollectionCount(1);
            int start2 = GC.CollectionCount(2);

            //Add items to the Reduced Collections Linked list.
            for (int x=1; x<=NUMBER_OF_ITEMS; x++)
                list.AddFirst(x);

            //Remove items from the list and validate the order/items
            for (int x = NUMBER_OF_ITEMS; x >= 1; x--)
            {
                if (x != list.First.Value)
                    Assert.Fail("Invalid values detected with using First and RemoveFirst");
                //Assert.AreEqual(x, list.First.Value);
                list.RemoveFirst();
            }

            //Sleep to allow some time for collections to occue
            Thread.Sleep(5000);

            //Check the collection counts to ensure no collections happened.
            int end0 = GC.CollectionCount(0);
            int end1 = GC.CollectionCount(1);
            int end2 = GC.CollectionCount(2);

            

            //tes if there were any collections
            Assert.AreEqual(start0, end0);
            Assert.AreEqual(start1, end1);
            Assert.AreEqual(start2, end2);


            //Test if we get an exception on RemoveFirst when list is empty
            bool? testExceptionOnEmpty = null;

            try
            {
                list.RemoveFirst();
                testExceptionOnEmpty = false;
            }
            catch (InvalidOperationException)
            {
                testExceptionOnEmpty = true;

            }

            Assert.IsTrue(testExceptionOnEmpty.Value,"RemoveFirst exception was not thrown when list empty");

            
        }

        

        
    }
}
