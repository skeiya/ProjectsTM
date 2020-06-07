using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TaskManagement.Model;
using TaskManagement.ViewModel;

namespace TaskManagement.Service
{
    public class WorkItemEditService
    {
        private readonly ViewData _viewData;
        private readonly UndoService _undoService;

        public WorkItemEditService(ViewData viewData, UndoService undoService)
        {
            this._viewData = viewData;
            this._undoService = undoService;
        }

        public void Add(WorkItems wis)
        {
            if (wis == null) return;
            var items = _viewData.Original.WorkItems;
            foreach (var w in wis)
            {
                if (items.Contains(w)) continue;
                items.Add(w);
                _undoService.Add(w);
            }
            _undoService.Push();
        }

        public void Add(WorkItem wi)
        {
            if (wi == null) return;
            var items = _viewData.Original.WorkItems;
            if (items.Contains(wi)) return;
            items.Add(wi);
            _undoService.Add(wi);
            _undoService.Push();
        }

        internal void Delete()
        {
            _viewData.Original.WorkItems.Remove(_viewData.Selected);
            _undoService.Delete(_viewData.Selected);
            _undoService.Push();
        }

        internal void Divide(WorkItem selected, int divided, int remain)
        {
            var d1 = selected.Clone();
            var d2 = selected.Clone();

            d1.Period.To = _viewData.Original.Callender.ApplyOffset(d1.Period.To, -remain);
            d2.Period.From = _viewData.Original.Callender.ApplyOffset(d2.Period.From, divided);

            _undoService.Delete(selected);
            _undoService.Add(d1);
            _undoService.Add(d2);
            _undoService.Push();

            var workItems = _viewData.Original.WorkItems;
            _viewData.Selected = null;
            workItems.Remove(selected);
            workItems.Add(d1);
            workItems.Add(d2);
        }

        internal void Replace(WorkItems before, WorkItems after)
        {
            _viewData.Original.WorkItems.Remove(before);
            _viewData.Original.WorkItems.Add(after);
            _undoService.Delete(before);
            _undoService.Add(after);
            _undoService.Push();
        }

        internal void Replace(WorkItem before, WorkItem after)
        {
            if (before.Equals(after)) return;
            _viewData.Original.WorkItems.Remove(before);
            _viewData.Original.WorkItems.Add(after);
            _undoService.Delete(before);
            _undoService.Add(after);
            _undoService.Push();
        }

        internal void Done(WorkItems selected)
        {
            var done = selected.Clone();

            foreach (var w in done) w.State = TaskState.Done;

            _undoService.Delete(selected);
            _undoService.Add(done);
            _undoService.Push();

            var workItems = _viewData.Original.WorkItems;
            _viewData.Selected = null;
            workItems.Remove(selected);
            workItems.Add(done);
        }

        internal void SelectAfterward(WorkItems starts)
        {
            var newSetected = new WorkItems();
            foreach (var s in starts)
            {
                newSetected.Add(GetSameMemberAfterItems(s));
            }
            _viewData.Selected = newSetected;
        }

        private WorkItems GetSameMemberAfterItems(WorkItem s)
        {
            var result = new WorkItems();
            foreach (var w in _viewData.GetFilteredWorkItemsOfMember(s.AssignedMember))
            {
                if (_viewData.Original.Callender.GetOffset(s.Period.From, w.Period.From) >= 0)
                {
                    if (result.Contains(w)) continue;
                    result.Add(w);
                }
            }
            return result;
        }

        internal bool AlignAfterward()
        {
            var starts = _viewData.Selected;
            if (starts == null) return false;
            if (HasSameMember(starts)) return false;
            var before = new WorkItems();
            var after = new WorkItems();
            var cal = _viewData.Original.Callender;
            foreach (var s in starts)
            {
                var lastTo = s.Period.To;
                foreach (var w in GetSameMemberAfterItems(s).OrderBy(o => o.Period.From))
                {
                    if (w.Equals(s)) continue;
                    before.Add(w.Clone());
                    var nextFrom = cal.ApplyOffset(lastTo, 1);
                    var offset = cal.GetOffset(w.Period.From, nextFrom);
                    var a = w.Clone();
                    a.Period = w.Period.ApplyOffset(offset, cal);
                    if (a.Period == null)
                    {
                        return false;
                    }
                    lastTo = a.Period.To;
                    after.Add(a);
                }
            }
            Replace(before, after);
            return true;
        }

        internal void AlignSelected()
        {
            if (_viewData.Selected == null) return;
            var before = _viewData.Selected.Clone().OrderBy(w => w.Period.From);
            var after = new WorkItems();
            var isFirst = true;
            CallenderDay lastTo = null;
            var cal = _viewData.Original.Callender;
            foreach (var w in before)
            {
                if (isFirst)
                {
                    isFirst = false;
                    lastTo = w.Period.To;
                    after.Add(w);
                    continue;
                }
                var nextFrom = cal.ApplyOffset(lastTo, 1);
                var offset = cal.GetOffset(w.Period.From, nextFrom);
                var newWorkItem = w.Clone();
                newWorkItem.Period = w.Period.ApplyOffset(offset, cal);
                lastTo = newWorkItem.Period.To;
                after.Add(newWorkItem);
            }
            Replace(_viewData.Selected, after);
            _viewData.Selected = after;
        }

        private static bool HasSameMember(WorkItems starts)
        {
            var members = new List<Member>();
            foreach (var s in starts)
            {
                if (members.Contains(s.AssignedMember)) return true;
                members.Add(s.AssignedMember);
            }
            return false;
        }

        internal void DivideInto2Parts()
        {
            DivideCore(false);
        }

        internal void MakeHalf()
        {
            DivideCore(true);
        }

        internal void DivideCore(bool makeHalf)
        {
            if (_viewData.Selected == null) return;
            var add = new WorkItems();
            var divided = new WorkItems();
            var callender = _viewData.Original.Callender;
            foreach (var w in _viewData.Selected)
            {
                var period = w.Period;
                var offset = callender.GetOffset(period.From, period.To);
                if (offset < 1) continue;

                var w1 = w.Clone();
                w1.Period.To = callender.ApplyOffset(w.Period.From, offset / 2);
                add.Add(w1);
                if (!makeHalf)
                {
                    var w2 = w.Clone();
                    w2.Period.From = callender.ApplyOffset(w.Period.From, +offset / 2 + 1);
                    add.Add(w2);
                }
                divided.Add(w);
            }
            if (!divided.Any()) return;

            _undoService.Delete(divided);
            _undoService.Add(add);
            _undoService.Push();

            var workItems = _viewData.Original.WorkItems;
            workItems.Remove(divided);
            workItems.Add(add);
            _viewData.Selected = add;
        }
    }
}
