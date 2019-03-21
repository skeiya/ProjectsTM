using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagement;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCallenderTayFormat()
        {
            var day = new CallenderDay() { Year = 2019, Month = 1, Day = 1 };
            Assert.AreEqual<string>("2019/01/01", day.ToString());
        }

        [TestMethod]
        public void TestTeamMemberFormat()
        {
            var member = new Member("下村", "圭矢", "AA");
            Assert.AreEqual<string>("下圭(AA)", member.ToString());
        }
    }
}
