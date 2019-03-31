namespace TaskManagement
{
    public class AppData
    {
        public Callender Callender = new Callender();
        public Members Members = new Members();
        public WorkItems WorkItems = new WorkItems();

        public AppData()
        {
        }

        public override bool Equals(object obj)
        {
            var target = obj as AppData;
            if (target == null) return false;
            if (!Callender.Equals(target.Callender)) return false;
            if (!Members.Equals(target.Members)) return false;
            return WorkItems.Equals(target.WorkItems);
        }

        //private void SetupDummyData()
        //{
        //    Projects.Add(new Project("Z123"));
        //    Projects.Add(new Project("Y345"));

        //    var shimo = new Member("下村", "圭矢", "K");
        //    Members.Add(shimo);
        //    var hoge = new Member("hoge", "foo", "AB");
        //    Members.Add(hoge);
        //    var avd = new Member("avd", "rg", "AB");
        //    Members.Add(avd);

        //    {
        //        var rand = new Random(123);
        //        foreach (var i in Enumerable.Range(1, 95))
        //        {
        //            Members.Add(new Member(rand.Next().ToString(), (rand.Next()).ToString(), i.ToString()));
        //        }
        //    }

        //    foreach (var m in Enumerable.Range(3, 10))
        //    {
        //        for (int d = 1; d < 31; d++)
        //        {
        //            Callender.Add(new CallenderDay(2019, m, d));
        //        }
        //    }
        //    foreach (var m in Enumerable.Range(1, 2))
        //    {
        //        for (int d = 1; d < 31; d++)
        //        {
        //            Callender.Add(new CallenderDay(2020, m, d));
        //        }
        //    }

        //    WorkItems.Add(new WorkItem(Projects.Get("Z123"), "基礎料金", null, new Period(Callender.Get(2019, 4, 5), Callender.Get(2019, 5, 1), Callender), shimo));
        //    WorkItems.Add(new WorkItem(Projects.Get("Y345"), "インストーラ",null, new Period(Callender.Get(2019, 4, 5), Callender.Get(2019, 4, 6), Callender), hoge));
        //    {
        //        var rand = new Random(345);
        //        foreach (var m in Members)
        //        {
        //            var from = Callender.Days.First();
        //            while (true)
        //            {
        //                var to = Callender.ApplyOffset(from, 15);
        //                if (to == null) break;
        //                var period = new Period(from, to, Callender);
        //                var wi = new WorkItem(Projects.Get("Z123"), rand.Next().ToString(), null, period, m);
        //                WorkItems.Add(wi);
        //                from = Callender.ApplyOffset(to, 1);
        //                if (from == null) break;
        //            }
        //        }
        //    }
        //}

    }
}
