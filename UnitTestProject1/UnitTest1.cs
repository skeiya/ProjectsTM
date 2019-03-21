using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagement;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCallenderDayFormat()
        {
            var day = new CallenderDay(2019, 1, 1);
            Assert.AreEqual<string>("2019/01/01", day.ToString());
        }

        [TestMethod]
        public void TestTeamMemberFormat()
        {
            var member = new Member("下村", "圭矢", "AA");
            Assert.AreEqual<string>("下圭(AA)", member.ToString());
        }

        [TestMethod]
        public void TestCallernderDayEquals()
        {
            var c1 = new CallenderDay(2019, 3, 3);
            var c2 = new CallenderDay(2019, 3, 3);
            var c3 = new CallenderDay(2019, 3, 4);
            Assert.IsTrue(c1.Equals(c2));
            Assert.IsFalse(c1.Equals(c3));
        }

        [TestMethod]
        public void TestWorkItemFormat()
        {
            var wi = new WorkItem("Z123", "仕様検討", new Period(new CallenderDay(2019, 3, 20), new CallenderDay(2019, 3, 22), new DummyPeriodCalculator()));
            Assert.AreEqual<string>("仕様検討 Z123 3d", wi.ToString());
        }
    }
}
