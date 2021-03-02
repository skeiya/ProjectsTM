using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsTM.ViewModel;
using System.IO;
using System.Xml.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestFilter
    {
        [TestMethod]
        public void TestFilterLoad()
        {
            foreach (var file in Directory.GetFiles("Filters"))
            {
                var filter = Filter.FromXml(XElement.Load(file));
                Assert.IsTrue(filter.ShowMembers.Count == 2);
            }
        }
    }
}
