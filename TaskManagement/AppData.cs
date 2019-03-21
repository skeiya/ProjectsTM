using System.Collections.Generic;

namespace TaskManagement
{
    class AppData
    {
        public static Callender Callender = new Callender();
        public static Team Team = new Team();
        public static List<WorkItem> WorkItems = new List<WorkItem>();

        AppData()
        {
            WorkItems.Add(new WorkItem("Z123", "基礎料金", new Period(new CallenderDay(2019, 4, 5), new CallenderDay(2019, 5, 1), AppData.Callender)));
            WorkItems.Add(new WorkItem("Y345", "インストーラ", new Period(new CallenderDay(2019, 4, 5), new CallenderDay(2019, 4, 6), AppData.Callender)));
        }
    }
}
