using System.Collections.Generic;
using System.Drawing;

namespace TaskManagement
{
    internal class ViewData
    {
        public AppData Filtered { get; }
        private Filter _filter;
        public AppData Original { get; }

        public ColorConditions ColorConditions = new ColorConditions();

        public ViewData(AppData original)
        {
            if (original == null)
            {
                original = new AppData(true);
                ColorConditions.Add(new ColorCondition("下圭", Color.Green));
                ColorConditions.Add(new ColorCondition("Z123", Color.Red));
            }
            Original = original;
        }

        internal void SetFilter(Filter filter)
        {
            _filter = filter;
            Original.WorkItems.SetFilter(filter.WorkItem);
            Original.Callender.SetFilter(filter.Period);
        }

        internal int GetDaysCount()
        {
            return Original.Callender.FilteredDays.Count;
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
            return Original.WorkItems;
        }

        internal List<CallenderDay> GetFilteredDays()
        {
            return Original.Callender.FilteredDays;
        }
    }
}
