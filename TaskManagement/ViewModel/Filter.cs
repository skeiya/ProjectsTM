using System;
using System.Collections.Generic;
using TaskManagement.Model;

namespace TaskManagement.ViewModel
{
    public class Filter : IEquatable<Filter>
    {
        public Filter() { }
        public Filter(string v, Period period, Members hideMembers, bool enableFreeTimeMember)
        {
            WorkItem = v;
            Period = period;
            HideMembers = hideMembers;
            EnableFreeTimeMember = enableFreeTimeMember;
        }

        public Members HideMembers { get; set; }
        public Period Period { get; set; }
        public string WorkItem { get; set; }
        public bool EnableFreeTimeMember { get; set; }

        public bool Equals(Filter other)
        {
            return other != null &&
                   EqualityComparer<Members>.Default.Equals(HideMembers, other.HideMembers) &&
                   EqualityComparer<Period>.Default.Equals(Period, other.Period) &&
                   WorkItem == other.WorkItem &&
                   EnableFreeTimeMember == other.EnableFreeTimeMember;
        }

        public override int GetHashCode()
        {
            int hashCode = 69401243;
            hashCode = hashCode * -1521134295 + EqualityComparer<Members>.Default.GetHashCode(HideMembers);
            hashCode = hashCode * -1521134295 + EqualityComparer<Period>.Default.GetHashCode(Period);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WorkItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(EnableFreeTimeMember);
            return hashCode;
        }

        internal Filter Clone()
        {
            var result = new Filter();
            if (this.HideMembers != null) result.HideMembers = this.HideMembers.Clone();
            if (this.WorkItem != null) result.WorkItem = (string)this.WorkItem.Clone();
            if (this.Period != null) result.Period = this.Period.Clone();
            result.EnableFreeTimeMember = this.EnableFreeTimeMember;
            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Filter);
        }
    }
}