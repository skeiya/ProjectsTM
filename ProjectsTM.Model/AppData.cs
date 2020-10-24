using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ProjectsTM.Model
{
    public class AppData
    {
        static public int DataVersion = 5; // 互換性のなくなる変更をしたときにこの数字を増やす
        public int Version
        {
            set {; }
            get { return DataVersion; }
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
            xml.Add(new XElement(nameof(Version), Version));
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
            var target = obj as AppData;
            if (target == null) return false;
            if (!Callender.Equals(target.Callender)) return false;
            if (!Members.Equals(target.Members)) return false;
            if (!ColorConditions.Equals(target.ColorConditions)) return false;
            return WorkItems.Equals(target.WorkItems);
        }

        public static AppData FromXml(XElement xml)
        {
            var result = new AppData();
            result.Callender = Callender.FromXml(xml.Element(nameof(Callender)));
            result.Members = Members.FromXml(xml.Element(nameof(Members)));
            result.WorkItems = WorkItems.FromXml(xml.Element(nameof(WorkItems)));
            result.ColorConditions = ColorConditions.FromXml(xml.Element(nameof(ColorConditions)));
            result.MileStones = MileStones.FromXml(xml);
            result.AbsentInfo = AbsentInfo.FromXml(xml);
            return result;
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
