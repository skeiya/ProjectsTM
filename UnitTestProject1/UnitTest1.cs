using FreeGridControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ProjectsTM.Logic;
using ProjectsTM.Model;
using static FreeGridControl.GridControl;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestIntArrayForDesignShrink()
        {
            var ar = new IntArrayForDesign();
            ar.Add(5);
            ar.Add(5);
            ar.Add(5);
            ar.Add(5);
            ar.SetCount(2);
            Assert.AreEqual<int>(ar.Count, 2);
        }

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
            Assert.AreEqual<string>("下村 圭矢(AA)", member.ToString());
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
            var wi = new WorkItem(new Project("Z123"), "仕様検討", new Tags(new List<string> { "a", "b" }), new Period(new CallenderDay(2019, 3, 20), new CallenderDay(2019, 3, 22)), new Member("A", "B", "C"), TaskState.Active, "TestDescription");
            callender.Add(new CallenderDay(2019, 3, 20));
            callender.Add(new CallenderDay(2019, 3, 21));
            callender.Add(new CallenderDay(2019, 3, 22));
            Assert.AreEqual<string>("[仕様検討][Z123][A B(C)][a|b][Active]", wi.ToString());
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
            using (var writer = StreamFactory.CreateWriter(stream))
            using (var reader = StreamFactory.CreateReader(writer.BaseStream))
            {
                var orgApp = BuildDummyData();

                AppDataSerializer.WriteToStream(orgApp, writer);
                writer.Flush();
                stream.Position = 0;
                var loadedApp = AppDataSerializer.LoadFromStream(reader);
                Assert.AreEqual<AppData>(orgApp, loadedApp);
            }
        }

        private static AppData BuildDummyData()
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
                ichiro, TaskState.Active, "解説：オープン戦　vsエンジェルス"));
            orgApp.WorkItems.Add(new WorkItem(
                new Project("シーズン"),
                "対カブス",
                Tags.Parse("c|d"),
                new Period(new CallenderDay(2018, 6, 3), new CallenderDay(2018, 8, 5)),
                gozzila, TaskState.Active, "解説：シーズン　vsカブス"));

            orgApp.ColorConditions.Add(new ColorCondition("イチロー", Color.Blue, Color.Black));

            orgApp.MileStones.Add(new MileStone("all star", new CallenderDay(2018, 6, 3), Color.AliceBlue));
            return orgApp;
        }

        [TestMethod]
        public void RSExport()
        {
            var appData = new AppData();
            var result = RSFileExporter.MakeTextAllData(appData);
            var expect = "Com\tMem\tProj\t" + Environment.NewLine;
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void XML()
        {
            using (var stream = new MemoryStream())
            using (var writer = StreamFactory.CreateWriter(stream))
            using (var reader = StreamFactory.CreateReader(writer.BaseStream))
            {
                var orgApp = BuildDummyData();

                var xmlSerializer1 = new XmlSerializer(typeof(AppData));
                xmlSerializer1.Serialize(writer, orgApp);
                writer.Flush();
                stream.Position = 0;
                var loadedApp = (AppData)xmlSerializer1.Deserialize(reader);
                Assert.AreEqual(orgApp, loadedApp);
            }
        }

        [TestMethod]
        public void DesirializeReternCode()
        {
            var w = new WorkItem(new Project("Z123"), "仕様検討", new Tags(new List<string> { "a", "b" }), new Period(new CallenderDay(2019, 3, 20), new CallenderDay(2019, 3, 22)), new Member("A", "B", "C"), TaskState.Active, "TestDescription");
            w.Description = "aaa" + Environment.NewLine + "bbb";
            var c = w.Clone();
            Assert.AreEqual(w.Description, c.Description);
        }

        [TestMethod]
        public void Client2Raw()
        {
            GridControl g = GetGrid();
            Assert.AreEqual(new RawPoint(10, 20), g.Client2Raw(new Point(10, 20)));
            Assert.AreEqual(new RawPoint(10, 450), g.Client2Raw(new Point(10, 450)));
            Assert.AreEqual(new RawPoint(250, 20), g.Client2Raw(new Point(250, 20)));
            Assert.AreEqual(new RawPoint(250, 450), g.Client2Raw(new Point(250, 450)));
            g.LockUpdate = true;
            g.VOffset = 6;
            g.HOffset = 7;
            g.LockUpdate = false;
            Assert.AreEqual(new RawPoint(10, 20), g.Client2Raw(new Point(10, 20)));
            Assert.AreEqual(new RawPoint(10, 450 + 6), g.Client2Raw(new Point(10, 450)));
            Assert.AreEqual(new RawPoint(250 + 7, 20), g.Client2Raw(new Point(250, 20)));
            Assert.AreEqual(new RawPoint(250 + 7, 450 + 6), g.Client2Raw(new Point(250, 450)));
        }

        [TestMethod]
        public void IsFixedArea()
        {
            GridControl g = GetGrid();
            Assert.IsTrue(g.IsFixedArea(new Point(10, 20)));
            Assert.IsTrue(g.IsFixedArea(new Point(10, 450)));
            Assert.IsTrue(g.IsFixedArea(new Point(250, 20)));
            Assert.IsFalse(g.IsFixedArea(new Point(250, 450)));
        }

        private static GridControl GetGrid()
        {
            var g = new GridControl();
            g.ColCount = 4;
            g.FixedColCount = 2;
            g.RowCount = 2;
            g.FixedRowCount = 1;
            g.ColWidths[0] = 100;
            g.ColWidths[1] = 100;
            g.ColWidths[2] = 200;
            g.ColWidths[3] = 200;
            g.RowHeights[0] = 150;
            g.RowHeights[1] = 300;
            g.LockUpdate = false;
            return g;
        }
    }
}
