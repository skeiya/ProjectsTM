using ProjectsTM.Model;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.Service
{
    public static class OverwrapedWorkItemsCollectService
    {
        public static List<WorkItem> Get(WorkItem target, WorkItems workItems)
        {
            var result = new List<WorkItem>();
            var workItemsOfMember = workItems.OfMember(target.AssignedMember);
            foreach (var wi in workItemsOfMember)
            {
                if (!wi.Period.HasInterSection(target.Period)) continue;
                if (wi.Equals(target)) continue;
                result.Add(wi);
                if (!result.Contains(target)) result.Add(target);
            }
            return result;
        }

        public static List<WorkItem> Get(WorkItems workItems)
        {
            var result = new List<WorkItem>();
            foreach (var members in workItems.EachMembers)
            {
                members.SortByPeriodStartDate();
                var ar = members.ToArray();
                for (var idx1 = 0; idx1 < members.Count; idx1++)
                {
                    for (var idx2 = idx1 + 1; idx2 < members.Count; idx2++)
                    {
                        var p1 = ar[idx1].Period;
                        var p2 = ar[idx2].Period;
                        if (p2.From <= p1.To)
                        {
                            Add(result, ar[idx1], ar[idx2]);
                        }
                    }
                }
            }
            return result;
        }

        private static void Add(List<WorkItem> result, WorkItem w1, WorkItem w2)
        {
            if (!result.Any(r => r.Equals(w1))) result.Add(w1);
            if (!result.Any(r => r.Equals(w2))) result.Add(w2);
        }
    }
}
