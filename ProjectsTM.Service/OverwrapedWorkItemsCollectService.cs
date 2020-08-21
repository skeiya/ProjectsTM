using ProjectsTM.Model;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.Service
{
    public static class OverwrapedWorkItemsCollectService
    {
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
                        if (p2.From <= p1.To) result.Add(ar[idx1]);
                    }
                }
            }
            return result;
        }
    }
}
