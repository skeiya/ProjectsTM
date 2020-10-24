using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsTM.Model;

namespace UnitTestProject
{
    [TestClass]
    public class SerializeTest
    {
        [TestMethod]
        public void AbsentInfoTest()
        {
            var info = new AbsentInfo();
            var term1 = new AbsentTerm(new Member("a", "b", "c"), new Period(new CallenderDay(1, 2, 3), new CallenderDay(4, 5, 6)));
            var term2 = new AbsentTerm(new Member("ad", "be", "cf"), new Period(new CallenderDay(11, 21, 31), new CallenderDay(41, 51, 61)));
            info.Add(term1);
            info.Add(term2);
            var xml = info.ToXml();
            var actual = AbsentInfo.FromXml(xml);
            Assert.AreEqual(info, actual);
        }

        [TestMethod]
        public void CallenderText()
        {
            var cal = new Callender();
            cal.Add(new CallenderDay(1, 2, 3));
            cal.Add(new CallenderDay(4, 5, 6));
            var xml = cal.ToXml();
            var actual = Callender.FromXml(xml);
            Assert.AreEqual(cal, actual);
        }
    }
}
