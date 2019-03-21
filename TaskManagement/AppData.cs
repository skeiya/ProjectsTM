using System.Collections.Generic;

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

            for (int m = 3; m < 8; m++)
            {
                for (int d = 1; d < 31; d++)
                {
                    Callender.Add(new CallenderDay(2019, m, d));
                }
            }

            WorkItems.Add(new WorkItem(Projects.Get("Z123"), "基礎料金", new Period(Callender.Get(2019, 4, 5), Callender.Get(2019, 5, 1), Callender), shimo));
            WorkItems.Add(new WorkItem(Projects.Get("Y345"), "インストーラ", new Period(Callender.Get(2019, 4, 5), Callender.Get(2019, 4, 6), Callender), hoge));
        }
    }
}
