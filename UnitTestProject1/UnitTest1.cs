using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            var callender = new Callender();
            var wi = new WorkItem(new Project("Z123"), "仕様検討", new Tags(new List<string> { "a", "b" }), new Period(new CallenderDay(2019, 3, 20), new CallenderDay(2019, 3, 22)), new Member("A", "B", "C"));
            callender.Add(new CallenderDay(2019, 3, 20));
            callender.Add(new CallenderDay(2019, 3, 21));
            callender.Add(new CallenderDay(2019, 3, 22));
            Assert.AreEqual<string>("[仕様検討][Z123][AB(C)][a|b][3d]", wi.ToString(callender));
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

        [TestMethod]
        public void LoadFile()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new StreamReader(writer.BaseStream))
            {
                var orgApp = new AppData();
                var ichiro = new Member("鈴木", "イチロー", "マリナーズ");
                var gozzila = new Member("松井", "秀喜", "ヤンキース");
                orgApp.Members.Add(ichiro);
                orgApp.Members.Add(gozzila);
                orgApp.Callender.Add(new CallenderDay(2018, 4, 1));
                orgApp.Callender.Add(new CallenderDay(2018, 5, 2));
                orgApp.Callender.Add(new CallenderDay(2018, 6, 3));
                orgApp.Callender.Add(new CallenderDay(2018, 7, 4));
                orgApp.Callender.Add(new CallenderDay(2018, 8, 5));
                orgApp.WorkItems.Add(new WorkItem(
                    new Project("オープン戦"),
                    "対エンジェルス",
                    Tags.Parse("a|b"),
                    new Period(new CallenderDay(2018, 4, 1), new CallenderDay(2018, 5, 2)),
                    ichiro));
                orgApp.WorkItems.Add(new WorkItem(
                    new Project("シーズン"),
                    "対カブス",
                    Tags.Parse("c|d"),
                    new Period(new CallenderDay(2018,6,3), new CallenderDay(2018,8,5)),
                    gozzila));

                orgApp.ColorConditions.Add(new ColorCondition("イチロー", Color.Blue));

                AppDataSerializer.WriteToStream(orgApp, writer);
                writer.Flush();
                stream.Position = 0;
                var loadedApp = AppDataSerializer.LoadFromStream(out string error, reader);
                Assert.AreEqual(orgApp, loadedApp);
            }
        }

        [TestMethod]
        public void RSExport()
        {
            var appData = new AppData();
            var result = RSFileExporter.MakeText(appData);
            var expect = "";
            Assert.AreEqual(expect, result);
        }
    }
}
