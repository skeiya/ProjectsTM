using ProjectsTM.Logic;
using ProjectsTM.Model;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProjectsTM.Service
{
    public class DummyDataService
    {
        public static void Save(string fileName)
        {
            var appData = new AppData();
            appData.Callender = new Callender();
            var periods = new List<Period>();
            var count = 0;
            var from = new CallenderDay(2019, 1, 1);
            for (var year = 2019; year < 2022; year++)
            {
                for (var month = 1; month < 13; month++)
                {
                    for (var day = 1; day < 31; day++)
                    {
                        appData.Callender.Add(new CallenderDay(year, month, day));
                        if (from == null)
                        {
                            from = new CallenderDay(year, month, day);
                            continue;
                        }
                        if (count > 3)
                        {
                            count = 0;
                            var to = new CallenderDay(year, month, day);
                            periods.Add(new Period(from, to));
                            from = null;
                        }
                        count++;
                    }
                }
            }
            appData.Members = new Members();
            for (var member = 00; member < 100; member++)
            {
                appData.Members.Add(new Member(member.ToString(), (100 - member).ToString(), "A", MemberState.Woking));
            }
            appData.WorkItems = new WorkItems();
            var p = new Project("PRJ");
            var t = new Tags(new List<string>() { "a" });
            foreach (var period in periods)
            {
                foreach (var m in appData.Members)
                {
                    appData.WorkItems.Add(new WorkItem(p, "Task", t, period, m, TaskState.Active, "DummyDescription"));
                }
            }

            var xml = new XmlSerializer(typeof(AppData));
            using (var w = StreamFactory.CreateWriter(fileName))
            {
                xml.Serialize(w, appData);
            }
        }
    }
}
