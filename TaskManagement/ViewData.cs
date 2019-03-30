using System.Collections.Generic;
using System.Drawing;

namespace TaskManagement
{
    internal class ViewData
    {
        public AppData Filtered { get; }
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
            Original.WorkItems.SetFilter(filter.WorkItem);
            Original.Callender.SetFilter(filter.Period);
            //Original.Members.SetFilter(filter.Members);
        }

        internal int GetDaysCount()
        {
            return Original.Callender.FilteredDays.Count;
        }

        internal Members GetFilteredMembers()
        {
            return Original.Members;
        }

        internal WorkItems GetFilteredWorkItems()
        {
            return Original.WorkItems;
        }

        internal Members GetFilterdMembers()
        {
            return Original.Members;
        }

        internal List<CallenderDay> GetFilteredDays()
        {
            return Original.Callender.FilteredDays;
        }
    }
}
