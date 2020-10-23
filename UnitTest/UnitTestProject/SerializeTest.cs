using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsTM.Model;

namespace UnitTestProject
{
    [TestClass]
    public class SerializeTest
    {
        [TestMethod]
        public void AbsentInfo()
        {
            var info = new AbsentInfo();
            var term1 = new AbsentTerm(new Member("a", "b", "c"), new Period(new CallenderDay(1, 2, 3), new CallenderDay(4, 5, 6)));
            var term2 = new AbsentTerm(new Member("ad", "be", "cf"), new Period(new CallenderDay(11, 21, 31), new CallenderDay(41, 51, 61)));
            info.Add(term1);
            info.Add(term2);
            var xml = info.ToXml();
            var actual = ProjectsTM.Model.AbsentInfo.FromXml(xml);
            Assert.AreEqual(info, actual);
        }
    }
}
