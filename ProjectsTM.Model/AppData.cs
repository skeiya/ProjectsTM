using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class AppData
    {
        public static readonly int DataVersion = 5; // 互換性のなくなる変更をしたときにこの数字を増やす

        public static AppData Dummy
        {
            get
            {
                var appData = new AppData();
                var ichiro = new Member("鈴木", "イチロー", "マリナーズ");
                var gozzila = new Member("松井", "秀喜", "ヤンキース");
                var godchild = new Member("田中", "将大", "ヤンキース");
                appData.Members.Add(ichiro);
                appData.Members.Add(gozzila);
                appData.Members.Add(godchild);
                appData.Callender.Add(new CallenderDay(2018, 4, 1));
                appData.Callender.Add(new CallenderDay(2018, 4, 2));
                appData.Callender.Add(new CallenderDay(2018, 4, 3));
                appData.Callender.Add(new CallenderDay(2018, 5, 2));
                appData.Callender.Add(new CallenderDay(2018, 6, 3));
                appData.Callender.Add(new CallenderDay(2018, 7, 4));
                appData.Callender.Add(new CallenderDay(2018, 7, 5));
                appData.Callender.Add(new CallenderDay(2018, 7, 6));
                appData.Callender.Add(new CallenderDay(2018, 7, 7));
                appData.Callender.Add(new CallenderDay(2018, 8, 5));
                appData.Callender.Add(new CallenderDay(2018, 8, 6));
                appData.Callender.Add(new CallenderDay(2020, 11, 29));
                appData.Callender.Add(new CallenderDay(2020, 12, 20));
                appData.Callender.Add(new CallenderDay(2021, 2, 13));
                var i = new WorkItem(
                    new Project("PrjA"),
                    "NameA",
                    Tags.Parse(string.Empty),
                    new Period(new CallenderDay(2018, 4, 1), new CallenderDay(2018, 5, 2)),
                    ichiro,
                    TaskState.Active,
                    string.Empty);
                var g = new WorkItem(
                    new Project("PrjB"),
                    "NameB", 
                    Tags.Parse(string.Empty),
                    new Period(new CallenderDay(2018, 6, 3), new CallenderDay(2018, 8, 5)),
                    gozzila, 
                    TaskState.Active, 
                    string.Empty);
                var c = new WorkItem(
                    new Project("PrjC"), 
                    "NameC",
                    Tags.Parse(string.Empty),
                    new Period(new CallenderDay(2018, 6, 3), new CallenderDay(2020, 12, 20)),
                    godchild, 
                    TaskState.Active, 
                    string.Empty);
                appData.WorkItems.Add(i);
                appData.WorkItems.Add(g);
                appData.WorkItems.Add(c);
                return appData;
            }
        }

        public Callender Callender = new Callender();
        public Members Members = new Members();
        public WorkItems WorkItems = new WorkItems();
        public ColorConditions ColorConditions = new ColorConditions();
        public MileStones MileStones = new MileStones();
        public AbsentInfo AbsentInfo = new AbsentInfo();

        public AppData()
        {
        }

        public XElement ToXml()
        {
            var xml = new XElement(nameof(AppData));
            xml.Add(new XElement("Version", DataVersion));
            xml.Add(Callender.ToXml());
            xml.Add(Members.ToXml());
            xml.Add(WorkItems.ToXml());
            xml.Add(ColorConditions.ToXml());
            xml.Add(MileStones.ToXml());
            xml.Add(AbsentInfo.ToXml());
            return xml;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AppData target)) return false;
            if (!Callender.Equals(target.Callender)) return false;
            if (!Members.Equals(target.Members)) return false;
            if (!ColorConditions.Equals(target.ColorConditions)) return false;
            return WorkItems.Equals(target.WorkItems);
        }

        public static AppData FromXml(XElement xml)
        {
            var version = GetVersion(xml);

            var result = new AppData();
            if (version < 5)
            {
                foreach (var callenderDay in xml.Element("Callender").Element("Days").Elements("CallenderDay"))
                {
                    var ca = CallenderDay.Parse(callenderDay.Element("Date").Value);
                    result.Callender.Add(ca);
                }
            }
            else
            {
                result.Callender = Callender.FromXml(xml.Element(nameof(Callender)));
            }
            result.Members = Members.FromXml(xml.Element(nameof(Members)));
            result.WorkItems = WorkItems.FromXml(xml.Element(nameof(WorkItems)), version);
            result.ColorConditions = ColorConditions.FromXml(xml.Element(nameof(ColorConditions)));
            result.MileStones = MileStones.FromXml(xml.Element(nameof(MileStones)), version);
            if (xml.Element(nameof(AbsentInfo)) != null)
            {
                result.AbsentInfo = AbsentInfo.FromXml(xml.Element(nameof(AbsentInfo)));
            }
            return result;
        }

        private static int GetVersion(XElement xml)
        {
            if (!xml.Elements("Version").Any()) return 0;
            return int.Parse(xml.Elements("Version").Single().Value);
        }

        public override int GetHashCode()
        {
            var hashCode = 1155948461;
            hashCode = hashCode * -1521134295 + EqualityComparer<Callender>.Default.GetHashCode(Callender);
            hashCode = hashCode * -1521134295 + EqualityComparer<Members>.Default.GetHashCode(Members);
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorConditions>.Default.GetHashCode(ColorConditions);
            hashCode = hashCode * -1521134295 + EqualityComparer<WorkItems>.Default.GetHashCode(WorkItems);
            return hashCode;
        }
    }
}
