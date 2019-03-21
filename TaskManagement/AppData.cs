using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement
{
    class AppData
    {
        public Callender Callender = new Callender();
        public Members Members = new Members();
        public List<WorkItem> WorkItems = new List<WorkItem>();
        public Projects Projects = new Projects();

        public AppData()
        {
            SetupDummyData();
        }

        private void SetupDummyData()
        {
            Projects.Add("Z123");
            Projects.Add("Y345");

            var shimo = new Member("下村", "圭矢", "K");
            Members.Add(shimo);
            var hoge = new Member("hoge", "foo", "AB");
            Members.Add(hoge);
            var avd = new Member("avd", "rg", "AB");
            Members.Add(avd);

            var rand = new Random(123);
            foreach(var i in Enumerable.Range(1, 95))
            {
                Members.Add(new Member(rand.Next().ToString(), (rand.Next()).ToString(), i.ToString()));
            }

            foreach (var m in Enumerable.Range(3, 10))
            {
                for (int d = 1; d < 31; d++)
                {
                    Callender.Add(new CallenderDay(2019, m, d));
                }
            }
            foreach (var m in Enumerable.Range(1, 2))
            {
                for (int d = 1; d < 31; d++)
                {
                    Callender.Add(new CallenderDay(2020, m, d));
                }
            }

            WorkItems.Add(new WorkItem(Projects.Get("Z123"), "基礎料金", new Period(Callender.Get(2019, 4, 5), Callender.Get(2019, 5, 1), Callender), shimo));
            WorkItems.Add(new WorkItem(Projects.Get("Y345"), "インストーラ", new Period(Callender.Get(2019, 4, 5), Callender.Get(2019, 4, 6), Callender), hoge));
        }
    }
}
