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
            result.HideMembers = this.HideMembers.Clone();
            result.WorkItem = (string)this.WorkItem.Clone();
            if (this.Period != null) result.Period = this.Period.Clone();
            return result;
        }
    }
}