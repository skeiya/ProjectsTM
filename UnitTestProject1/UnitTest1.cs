using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            var wi = new WorkItem(new Project("Z123"), "仕様検討", new Tags(new List<string> { "a", "b" }), new Period(new CallenderDay(2019, 3, 20), new CallenderDay(2019, 3, 22), new DummyPeriodCalculator()), new Member("A", "B", "C"));
            Assert.AreEqual<string>("[仕様検討][Z123][AB(C)][a|b][3d]", wi.ToString());
        }

        [TestMethod]
        public void TestRegex()
        {
            var pattern = "Z123";
            var target = "[基礎料金][Z123][下圭(K)][27d]";
            Assert.IsTrue(Regex.IsMatch(target, pattern));
        }

        [TestMethod]
        public void CallenderDayParse()
        {
            var text = "2019/3/10";
            var c = CallenderDay.Parse(text);
            Assert.AreEqual(new CallenderDay(2019, 3, 10), c);
        }
    }
}
