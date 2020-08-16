using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectsTM.Service
{
    public class AuditService
    {
        private WorkItems _workitems;
        private Callender _callender;
        private Task<List<TaskListItem>> _task;
        public bool IsActive { get; set; }

        public Task<List<TaskListItem>> StartAuditTask(WorkItems workItems, Callender callender)
        {           
            _workitems = workItems;
            _callender = callender;
            _task?.Dispose();
            _task = Task.Run(() => { return AuditTask(); });
            return _task;
        }

        private List<TaskListItem> AuditTask()
        {
            IsActive = true;
            List<TaskListItem> err = GetAuditList();
            IsActive = false;
            return err;
        }

        private List<TaskListItem> GetAuditList()
        {
            Task<List<TaskListItem>> task1 = Task.Run(() =>
            {
                return OverwrapedWorkItemsCollectService.Get(_workitems).Select(w => CreateErrorItem(w, "期間重複")).ToList();
            });

            Task<List<TaskListItem>> task2 = Task.Run(() =>
            {
                return GetNotStartedErrorAndTooBigError();
            });

            bool allTasksCompleted = Task.WaitAll(new[] { task1, task2 }, 10000);
            return allTasksCompleted ? MergeErrorList(task1.Result, task2.Result) : null;
        }

        private List<TaskListItem> MergeErrorList(List<TaskListItem> errList1, List<TaskListItem> errList2)
        {
            var result = errList1;
            foreach (var i in errList2)
            {
                if (result.Any(l => l.WorkItem.Equals(i.WorkItem))) continue;
                result.Add(i);
            }
            return result;
        }

        public bool WorkitemsAndCallenderChanged(WorkItems workitems,Callender callender)
        {
            if (!_workitems.Equals(workitems) ||
                !_callender.Equals(callender)) return true;
            return false;
        }

        private List<TaskListItem> GetNotStartedErrorAndTooBigError()
        {
            var list = new List<TaskListItem>();
            foreach (var wi in _workitems)
            {
                if (list.Any(l => l.WorkItem.Equals(wi))) continue;
                if (IsNotStartedError(wi))
                {
                    list.Add(CreateErrorItem(wi, "未開始"));
                    continue;
                }
                if (IsTooBigError(wi))
                {
                    list.Add(CreateErrorItem(wi, "要分解"));
                    continue;
                }
            }
            return list;
        }

        private bool IsTooBigError(WorkItem wi)
        {
            if (wi.State == TaskState.Background) return false;
            if (wi.State == TaskState.Done) return false;
            if (!IsStartSoon(wi)) return false;
            return IsTooBig(wi);
        }

        private bool IsTooBig(WorkItem wi)
        {
            return 10 < _callender.GetPeriodDayCount(wi.Period);
        }

        private bool IsStartSoon(WorkItem wi)
        {
            var restPeriod = new Period(_callender.NearestFromToday, wi.Period.From);
            return _callender.GetPeriodDayCount(restPeriod) < 4;
        }

        private static bool IsNotStartedError(WorkItem wi)
        {
            if (IsStarted(wi)) return false;
            return CallenderDay.Today >= wi.Period.From;
        }

        private static bool IsStarted(WorkItem wi)
        {
            return wi.State != TaskState.New || wi.State == TaskState.Background;
        }

        private static TaskListItem CreateErrorItem(WorkItem wi, string msg)
        {
            return new TaskListItem(wi, Color.White, false, msg);
        }
    }
}
