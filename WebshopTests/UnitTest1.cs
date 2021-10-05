using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebshopTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int id = 20;

            Assert.AreEqual(20, id);
        }

        [TestMethod]
        public void TestMethod2()
        {
            int id = 20;

            Assert.AreEqual(25, id+5);
        }

        [TestMethod]
        public void TestMethod3()
        {
            int id = 20;

            Assert.AreNotEqual(25, id);
        }
    }
}
