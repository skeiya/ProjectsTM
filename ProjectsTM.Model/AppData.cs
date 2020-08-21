using System.Collections.Generic;

namespace ProjectsTM.Model
{
    public class AppData
    {
        public const int DataVersion = 2; // 互換性のなくなる変更をしたときにこの数字を増やす
        public int Version = DataVersion;
        public Callender Callender = new Callender();
        public Members Members = new Members();
        public WorkItems WorkItems = new WorkItems();
        public ColorConditions ColorConditions = new ColorConditions();
        public MileStones MileStones = new MileStones();

        public AppData()
        {
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
