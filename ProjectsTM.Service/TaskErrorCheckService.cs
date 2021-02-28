using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.Service
{
    public static class TaskErrorCheckService
    {
        public static Dictionary<WorkItem, string> GetAuditList(ViewData viewData)
        {
            var result = OverlapedWorkItemsCollectService.Get(viewData.Original.WorkItems).ToDictionary(w => w, _ => "衝突");
            CallenderDay soon = null;
            for (int i = 5; i >= 0; i--)
            {
                soon = viewData.Original.Callender.ApplyOffset(viewData.Original.Callender.NearestFromToday, i);
                if (soon != null) break;
            }
            foreach (var wi in viewData.FilteredItems.WorkItems)
            {
                if (result.TryGetValue(wi, out var dummy)) continue;
                if (IsNotEndError(wi))
                {
                    result.Add(wi, "未完");
                    continue;
                }
                if (IsTooBigError(wi, soon, viewData.Original.Callender))
                {
                    result.Add(wi, "過大");
                    continue;
                }
            }
            return result;
        }

        public static bool IsUserErrorExist(Member me, ViewData viewData)
        {
            var audit = GetAuditList(viewData);
            return audit.Any(wi => wi.Key.AssignedMember == me);
        }

        private static bool IsNotEndError(WorkItem wi)
        {
            if (wi.State == TaskState.Done) return false;
            if (wi.State == TaskState.Background) return false;
            return wi.Period.To < CallenderDay.Today;
        }

        private static bool IsTooBigError(WorkItem wi, CallenderDay soon, Callender callender)
        {
            if (wi.State == TaskState.Background) return false;
            if (wi.State == TaskState.Done) return false;
            if (!IsStartSoon(wi, soon)) return false;
            return IsTooBig(wi, callender);
        }

        private static bool IsTooBig(WorkItem wi, Callender callender)
        {
            return 10 < callender.GetPeriodDayCount(wi.Period);
        }

        private static bool IsStartSoon(WorkItem wi, CallenderDay soon)
        {
            return wi.Period.From <= soon;
        }
    }
}
