using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TaskManagement
{
    public class ViewData
    {
        public Filter Filter { get; private set; }
        public AppData Original { get; set; }
        private WorkItem _selected;
        public WorkItem Selected
        {
            get { return _selected; }
            set
            {
                var org = _selected;
                _selected = value;
                if (!_selected.Equals(org))
                {
                    SelectedWorkItemChanged(this, null);
                }
            }
        }


        public event EventHandler FilterChanged;
        public event EventHandler SelectedWorkItemChanged;

        public ViewData(AppData appData)
        {
            Original = appData;
        }

        public void SetFilter(Filter filter)
        {
            Filter = filter;
            FilterChanged(this, null);
        }

        public int GetDaysCount()
        {
            if (Filter == null || Filter.Period == null) return Original.Callender.Days.Count;
            return Original.Callender.GetPeriodDayCount(Filter.Period);
        }

        public Members GetFilteredMembers()
        {
            var result = new Members();
            if (Filter == null || Filter.FilteringMembers == null)
            {
                foreach (var m in this.Original.Members)
                {
                    result.Add(m);
                }
                return result;
            }

            foreach (var m in Original.Members)
            {
                if (Filter.FilteringMembers.Contain(m)) continue;
                result.Add(m);
            }
            return result;
        }

        public WorkItems GetFilteredWorkItems()
        {
            if (Filter == null) return Original.WorkItems;
            var filteredMembers = GetFilteredMembers();
            var period = GetFilteredDays();
            var result = new WorkItems();
            foreach (var w in Original.WorkItems)
            {
                if (!filteredMembers.Contain(w.AssignedMember)) continue;
                if (!string.IsNullOrEmpty(Filter.WorkItem))
                {
                    if (!Regex.IsMatch(w.ToString(Original.Callender), Filter.WorkItem)) continue;
                }
                result.Add(w);
            }
            return result;
        }

        public List<CallenderDay> GetFilteredDays()
        {
            if (Filter == null) return Original.Callender.Days;
            if (Filter.Period == null) return Original.Callender.Days;
            var result = new List<CallenderDay>();
            bool isFound = false;
            foreach (var d in Original.Callender.Days)
            {
                if (d.Equals(Filter.Period.From)) isFound = true;
                if (isFound) result.Add(d);
                if (d.Equals(Filter.Period.To)) return result;
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
