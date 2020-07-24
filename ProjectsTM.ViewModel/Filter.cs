using ProjectsTM.Model;
using System;
using System.Collections.Generic;

namespace ProjectsTM.ViewModel
{
    public class Filter : IEquatable<Filter>
    {
        public Filter() { }
        public Filter(string v, Period period, Members hideMembers, bool isFreeTimeMemberShow, MileStoneFilters mileStoneFilters)
        {
            if (v != null) WorkItem = v;
            if (period != null) Period = period;
            if (hideMembers != null) HideMembers = hideMembers;
            IsFreeTimeMemberShow = isFreeTimeMemberShow;
            if (mileStoneFilters != null) MileStoneFilters = mileStoneFilters;
        }

        public Members HideMembers { get; private set; } = new Members();
        public Period Period { get; set; } = new Period();
        public string WorkItem { get; set; } = string.Empty;
        public bool IsFreeTimeMemberShow { get; set; } = true;
        public MileStoneFilters MileStoneFilters { get; private set; } = new MileStoneFilters();
        public static Filter All => new Filter(null, null, new Members(), false, null);

        public bool Equals(Filter other)
        {
            return other != null &&
                   EqualityComparer<Members>.Default.Equals(HideMembers, other.HideMembers) &&
                   EqualityComparer<Period>.Default.Equals(Period, other.Period) &&
                   WorkItem == other.WorkItem &&
                   IsFreeTimeMemberShow == other.IsFreeTimeMemberShow &&
                   MileStoneFilters.Equals(other.MileStoneFilters);
        }

        public override int GetHashCode()
        {
            int hashCode = 69401243;
            hashCode = hashCode * -1521134295 + EqualityComparer<Members>.Default.GetHashCode(HideMembers);
            hashCode = hashCode * -1521134295 + EqualityComparer<Period>.Default.GetHashCode(Period);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WorkItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(IsFreeTimeMemberShow);
            hashCode = hashCode * -1521134295 + EqualityComparer<MileStoneFilters>.Default.GetHashCode(MileStoneFilters);
            return hashCode;
        }

        public Filter Clone()
        {
            var result = Filter.All;
            result.HideMembers = this.HideMembers.Clone();
            result.WorkItem = (string)this.WorkItem.Clone();
            result.Period = this.Period.Clone();
            result.IsFreeTimeMemberShow = this.IsFreeTimeMemberShow;
            result.MileStoneFilters = this.MileStoneFilters.Clone();
            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Filter);
        }
    }
}