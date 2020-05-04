using System.Collections.Generic;
using TaskManagement.Model;

namespace TaskManagement.Logic
{
    public static class OverwrapedWorkItemsGetter
    {
        public static List<WorkItem> Get(WorkItems workItems)
        {
            var result = new List<WorkItem>();
            foreach (var members in workItems.EachMembers)
            {
                foreach (var src in members)
                {
                    foreach (var dst in members)
                    {
                        if (!src.AssignedMember.Equals(dst.AssignedMember)) continue;
                        if (!src.Period.HasInterSection(dst.Period)) continue;
                        if (src.Equals(dst)) continue;
                        result.Add(src);
                    }
                }
            }
            return result;
        }
    }
}
