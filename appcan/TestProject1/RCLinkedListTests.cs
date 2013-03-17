/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCLibrary;

namespace TestProject1
{
    public class TestClass
    {
        public string File;

    }

    [TestClass]
    public class RCLinkedListTests
    {
        [TestMethod]
        public void TestRCLInkedListFind()
        {
            TestClass a = new TestClass();
            TestClass b = new TestClass();
            TestClass c = new TestClass();

            a.File = "a";
            b.File = "b";
            c.File = "c";

            RCLinkedList<TestClass> classes = new RCLinkedList<TestClass>();
            classes.AddFirst(a);
            classes.AddFirst(b);
            classes.AddFirst(c);

            TestClass find1=classes.Find(d=>d.File=="a");
            Assert.AreEqual(find1, a);

            TestClass find2 = classes.Find(d => d.File == "b");
            Assert.AreEqual(find2, b);

            TestClass find3 = classes.Find(d => d.File == "c");
            Assert.AreEqual(find3, c);

            //Remove class b
            classes.Remove(b);

            //Test that everything is still there except "b"
            find1 = classes.Find(d => d.File == "a");
            Assert.AreEqual(find1, a);

            find2 = classes.Find(d => d.File == "b");
            Assert.AreEqual(find2, null);

            find3 = classes.Find(d => d.File == "c");
            Assert.AreEqual(find3, c);

        }
    }
}
