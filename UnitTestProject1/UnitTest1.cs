using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagement;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var day = new CallenderDay() { Year = 2019, Month = 1, Day = 1 };
            Assert.AreEqual<string>("2019 01 01", day.ToString());
        }
    }
}
