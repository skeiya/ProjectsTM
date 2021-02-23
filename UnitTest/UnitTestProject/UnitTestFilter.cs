using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsTM.ViewModel;
using System;
using System.Xml.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestFilter
    {
        [TestMethod]
        public void TestFilterLoad()
        {
            var filter = Filter.FromXml(XElement.Load("filterSample.xml"));
            Assert.IsTrue(filter.ShowMembers.Count == 2);
        }
    }
}
