using System;
using TaskManagement.Model;

namespace TaskManagement.ViewModel
{
    public class Filter
    {
        public Filter() { }
        public Filter(string v, Period period, Members hideMembers)
        {
            WorkItem = v;
            Period = period;
            HideMembers = hideMembers;
        }

        public Members HideMembers { get; set; }
        public Period Period { get; set; }
        public string WorkItem { get; set; }

        internal Filter Clone()
        {
            var result = new Filter();
            if (this.HideMembers != null) result.HideMembers = this.HideMembers.Clone();
            if (this.WorkItem != null) result.WorkItem = (string)this.WorkItem.Clone();
            if (this.Period != null) result.Period = this.Period.Clone();
            return result;
        }
    }
}