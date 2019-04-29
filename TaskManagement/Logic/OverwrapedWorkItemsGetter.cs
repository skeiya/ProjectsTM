using System.Collections.Generic;
using TaskManagement.Model;

namespace TaskManagement.Logic
{
    public class OverwrapedWorkItemsGetter
    {
        public static List<WorkItem> Get(WorkItems workItems)
        {
            var result = new List<WorkItem>();
            foreach (var src in workItems)
            {
                foreach (var dst in workItems)
                {
                    if (src.Equals(dst))
                    {
                        continue;
                    }
                    if (!src.AssignedMember.Equals(dst.AssignedMember)) continue;
                    if (src.Period.HasInterSection(dst.Period)) result.Add(src);
                }
            }
            return result;
        }
    }
}
