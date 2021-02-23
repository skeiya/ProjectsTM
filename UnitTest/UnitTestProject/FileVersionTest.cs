using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectsTM.Model;
using ProjectsTM.Service;
using System;
using System.IO;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class FileVersionTest
    {
        [TestMethod]
        public void TestVersion1()
        {
            var appData = AppDataSerializeService.Deserialize(Path.Combine("Version1", "oldformat.xml"));
            Assert.IsTrue(appData.Callender.Contains(new CallenderDay(2021, 1, 1)));
            Assert.IsTrue(appData.Members.Contains(new Member("a", "b", "c")));
            Assert.IsTrue(appData.WorkItems.Count() == 1);
        }
    }
}
