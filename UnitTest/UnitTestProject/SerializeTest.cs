﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void CallenderTest()
        {
            var cal = new Callender();
            cal.Add(new CallenderDay(1, 2, 3));
            cal.Add(new CallenderDay(4, 5, 6));
            var xml = cal.ToXml();
            var actual = Callender.FromXml(xml);
            Assert.AreEqual(cal, actual);
        }

        [TestMethod]
        public void MembersTest()
        {
            var members = new Members();
            members.Add(new Member("a", "b", "c"));
            members.Add(new Member("d", "e", "f"));
            var xml = members.ToXml();
            var actual = Members.FromXml(xml);
            Assert.AreEqual(members, actual);
        }

        [TestMethod]
        public void WorkItemsTest()
        {
            var workItems = new WorkItems();
            var project = new Project("a");
            var name = "b";
            var tags = new Tags(new System.Collections.Generic.List<string>() { "c" });
            var period = new Period(new CallenderDay(1, 2, 3), new CallenderDay(4, 5, 6));
            var member = new Member("d", "e", "f");
            var state = TaskState.Active;
            var description = "ggg\nhhh";
            var w = new WorkItem(project, name, tags, period, member, state, description);
            workItems.Add(w);

            var xml = workItems.ToXml();
            var actual = WorkItems.FromXml(xml, AppData.DataVersion);
            Assert.AreEqual(workItems, actual);
        }

        [TestMethod]
        public void ColorConditionsTest()
        {
            var conditions = new ColorConditions();
            conditions.Add(new ColorCondition("a", System.Drawing.Color.White, System.Drawing.Color.Black));
            conditions.Add(new ColorCondition("b", System.Drawing.Color.Red, System.Drawing.Color.Blue));
            var xml = conditions.ToXml();
            var actual = ColorConditions.FromXml(xml);
            Assert.AreEqual(conditions, actual);
        }

        [TestMethod]
        public void MileStonesTest()
        {
            var milestones = new MileStones();
            milestones.Add(new MileStone("a", new Project("b"), new CallenderDay(1, 2, 3), System.Drawing.Color.Gray, new MileStoneFilter("c"), TaskState.Background));
            milestones.Add(new MileStone("d", new Project("e"), new CallenderDay(11, 12, 13), System.Drawing.Color.Green, new MileStoneFilter("g"), TaskState.Done));
            var xml = milestones.ToXml();
            var actual = MileStones.FromXml(xml, AppData.DataVersion);
            Assert.AreEqual(milestones, actual);
        }
    }
}
