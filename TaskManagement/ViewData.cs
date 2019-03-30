using System.Collections.Generic;

namespace TaskManagement
{
    internal class ViewData
    {
        private Filter _filter;
        public AppData Original { get; }

        public ColorConditions ColorConditions = new ColorConditions();

        public ViewData()
        {
            Original = new AppData();
        }

        internal void SetFilter(Filter filter)
        {
            _filter = filter;
        }

        internal int GetDaysCount()
        {
            if (_filter == null || _filter.Period == null) return Original.Callender.Days.Count;
            return _filter.Period.Days.Count;
        }

        internal Members GetFilteredMembers()
        {
            if (_filter == null) return Original.Members;
            if (_filter.FilteringMembers == null) return Original.Members;
            var result = new Members();
            foreach (var m in Original.Members)
            {
                if (_filter.FilteringMembers.Contain(m)) continue;
                result.Add(m);
            }
            return result;
        }

        internal WorkItems GetFilteredWorkItems()
        {
            if (_filter == null) return Original.WorkItems;
            var filteredMembers = GetFilteredMembers();
            var period = GetFilteredDays();
            var result = new WorkItems();
            foreach (var w in Original.WorkItems)
            {
                //if(w.)
            }
            return result;
        }

        internal List<CallenderDay> GetFilteredDays()
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
    }
}
