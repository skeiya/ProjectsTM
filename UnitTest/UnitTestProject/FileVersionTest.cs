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
        public void TestVersions()
        {
            foreach (var v in Enumerable.Range(0, 6))
            {
                var appData = AppDataSerializeService.Deserialize(Path.Combine("Versions", "version" + v.ToString() + ".xml"));
                Assert.IsTrue(appData.Callender.Contains(new CallenderDay(2021, 1, 1)));
                Assert.IsTrue(appData.Members.Contains(new Member("a", "b", "c")));
                Assert.IsTrue(appData.WorkItems.Count() == 1);
            }
        }
    }
}
