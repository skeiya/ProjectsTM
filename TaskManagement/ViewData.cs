using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TaskManagement
{
    public class ViewData
    {
        private Filter _filter;
        public AppData Original { get; set; }
        public WorkItem Selected { get; set; }


        public event EventHandler FilterChanged;

        public ViewData(AppData appData)
        {
            Original = appData;
        }

        public void SetFilter(Filter filter)
        {
            _filter = filter;
            FilterChanged(this, null);
        }

        public int GetDaysCount()
        {
            if (_filter == null || _filter.Period == null) return Original.Callender.Days.Count;
            return _filter.Period.Days.Count;
        }

        public Members GetFilteredMembers()
        {
            var result = new Members();
            if (_filter == null || _filter.FilteringMembers == null)
            {
                foreach (var m in this.Original.Members)
                {
                    result.Add(m);
                }
                return result;
            }

            foreach (var m in Original.Members)
            {
                if (_filter.FilteringMembers.Contain(m)) continue;
                result.Add(m);
            }
            return result;
        }

        public WorkItems GetFilteredWorkItems()
        {
            if (_filter == null) return Original.WorkItems;
            var filteredMembers = GetFilteredMembers();
            var period = GetFilteredDays();
            var result = new WorkItems();
            foreach (var w in Original.WorkItems)
            {
                if (!filteredMembers.Contain(w.AssignedMember)) continue;
                if (!period.Contains(w.Period.From)) continue;
                if (!period.Contains(w.Period.To)) continue;
                if (!Regex.IsMatch(w.ToString(), _filter.WorkItem)) continue;
                result.Add(w);
            }
            return result;
        }

        public List<CallenderDay> GetFilteredDays()
        {
            if (_filter == null) return Original.Callender.Days;
            if (_filter.Period == null) return Original.Callender.Days;
            var result = new List<CallenderDay>();
            bool isFound = false;
            foreach (var d in Original.Callender.Days)
            {
                if (d.Equals(_filter.Period.From)) isFound = true;
                if (isFound) result.Add(d);
                if (d.Equals(_filter.Period.To)) return result;
            }
            return result;
        }

        public void UpdateCallenderAndMembers(WorkItem wi)
        {
            var days = Original.Callender.Days;
            if (!days.Contains(wi.Period.From)) days.Add(wi.Period.From);
            if (!days.Contains(wi.Period.To)) days.Add(wi.Period.To);
            days.Sort();
            if (!Original.Members.Contain(wi.AssignedMember)) Original.Members.Add(wi.AssignedMember);
        }
    }
}
