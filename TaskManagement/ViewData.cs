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

        internal void SetFilter(string text)
        {
            Original.WorkItems.SetFilter(text);
        }

        internal int GetDaysCount()
        {
            return Original.Callender.FilteredDays.Count;
        }

        internal void SetFilter(Period period)
        {
            Original.Callender.SetFilter(period);
        }

        internal Members GetFilteredMembers()
        {
            return Original.Members;
        }

        internal WorkItems GetFilteredWorkItems()
        {
            return Original.WorkItems;
        }

        internal void SetFilter(List<Member> filterMembers)
        {
            Original.Members.SetFilter(filterMembers);
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
