using System.Collections.Generic;

namespace TaskManagement
{
    class AppData
    {
        public static Callender Callender = new Callender();
        public static Team Team = new Team();
        public static List<WorkItem> WorkItems = new List<WorkItem>();
        public static Projects Projects = new Projects();

        AppData()
        {
            SetupDummyData();
        }

        private static void SetupDummyData()
        {
            Projects.Add("Z123");
            Projects.Add("Y345");

            WorkItems.Add(new WorkItem(Projects.Get("Z123"), "基礎料金", new Period(new CallenderDay(2019, 4, 5), new CallenderDay(2019, 5, 1), AppData.Callender)));
            WorkItems.Add(new WorkItem(Projects.Get("Y345"), "インストーラ", new Period(new CallenderDay(2019, 4, 5), new CallenderDay(2019, 4, 6), AppData.Callender)));
        }
    }
}
