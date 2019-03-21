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

            Members.Add(new Member("下村", "圭矢", "K"));
            Members.Add(new Member("hoge", "foo", "AB"));
            Members.Add(new Member("avd", "rg", "AB"));

            for (int m = 3; m < 8; m++)
            {
                for (int d = 1; d < 31; d++)
                {
                    Callender.Add(new CallenderDay(2019, m, d));
                }
            }

            WorkItems.Add(new WorkItem(Projects.Get("Z123"), "基礎料金", new Period(Callender.Get(2019, 4, 5), Callender.Get(2019, 5, 1), Callender)));
            WorkItems.Add(new WorkItem(Projects.Get("Y345"), "インストーラ", new Period(Callender.Get(2019, 4, 5), Callender.Get(2019, 4, 6), Callender)));
        }
    }
}
