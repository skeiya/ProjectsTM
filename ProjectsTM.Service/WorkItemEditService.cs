using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.Service
{
    public class WorkItemEditService
    {
        private readonly ViewData _viewData;

        public WorkItemEditService(ViewData viewData)
        {
            this._viewData = viewData;
        }

        public void Add(WorkItems wis)
        {
            if (wis == null) return;
            var items = _viewData.Original.WorkItems;
            foreach (var w in wis)
            {
                if (items.Contains(w)) continue;
                items.Add(w);
                _viewData.UndoService.Add(w);
            }
            _viewData.UndoService.Push();
        }

        public void Add(WorkItem wi)
        {
            if (wi == null) return;
            var items = _viewData.Original.WorkItems;
            if (items.Contains(wi)) return;
            items.Add(wi);
            _viewData.UndoService.Add(wi);
            _viewData.UndoService.Push();
        }

        public void Delete()
        {
            _viewData.Original.WorkItems.Remove(_viewData.Selected);
            _viewData.UndoService.Delete(_viewData.Selected);
            _viewData.Selected = new WorkItems();
            _viewData.UndoService.Push();
        }

        public void Divide(WorkItem selected, int divided, int remain)
        {
            var d1 = selected.Clone();
            var d2 = selected.Clone();

            d1.Period.To = _viewData.Original.Callender.ApplyOffset(d1.Period.To, -remain);
            d2.Period.From = _viewData.Original.Callender.ApplyOffset(d2.Period.From, divided);

            var workItems = _viewData.Original.WorkItems;
            _viewData.Selected = null;
            workItems.Remove(selected);
            workItems.Add(d1);
            workItems.Add(d2);

            _viewData.UndoService.Delete(selected);
            _viewData.UndoService.Add(d1);
            _viewData.UndoService.Add(d2);
            _viewData.UndoService.Push();
        }

        public void Replace(WorkItems before, WorkItems after)
        {
            _viewData.Original.WorkItems.Remove(before);
            _viewData.Original.WorkItems.Add(after);
            _viewData.UndoService.Delete(before);
            _viewData.UndoService.Add(after);
            _viewData.UndoService.Push();
        }

        public void Replace(WorkItem before, WorkItem after)
        {
            if (before.Equals(after)) return;
            _viewData.Original.WorkItems.Remove(before);
            _viewData.Original.WorkItems.Add(after);
            _viewData.UndoService.Delete(before);
            _viewData.UndoService.Add(after);
            _viewData.UndoService.Push();
        }

        public void ChangeState(WorkItems selected, TaskState state)
        {
            var newWis = selected.Clone();

            foreach (var w in newWis) w.State = state;

            var workItems = _viewData.Original.WorkItems;
            _viewData.Selected = null;
            workItems.Remove(selected);
            workItems.Add(newWis);

            _viewData.UndoService.Delete(selected);
            _viewData.UndoService.Add(newWis);
            _viewData.UndoService.Push();
        }

        public void SelectAfterward(WorkItems starts)
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
            foreach (var w in _viewData.FilteredItems.GetWorkItemsOfMember(s.AssignedMember))
            {
                if (_viewData.Original.Callender.GetOffset(s.Period.From, w.Period.From) >= 0)
                {
                    if (result.Contains(w)) continue;
                    result.Add(w);
                }
            }
            return result;
        }

        public bool AlignAfterward()
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

        public void AlignSelected()
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

        public void DivideInto2Parts()
        {
            DivideCore(false);
        }

        public void MakeHalf()
        {
            DivideCore(true);
        }

        public void DivideCore(bool makeHalf)
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

            var workItems = _viewData.Original.WorkItems;
            workItems.Remove(divided);
            workItems.Add(add);
            _viewData.Selected = add;

            _viewData.UndoService.Delete(divided);
            _viewData.UndoService.Add(add);
            _viewData.UndoService.Push();
        }

        internal void ShiftDays(int shift)
        {
            if (_viewData.Selected == null) return;
            if (_viewData.Selected.Count() != 1) return;

            var before = _viewData.Selected.Unique;
            var after = before.Clone();
            after.Period = after.Period.ApplyOffset(shift, _viewData.Original.Callender);

            if (after.Period == null) return;

            Replace(before, after);
            _viewData.Selected = new WorkItems(after);
        }

        internal void ExpandDays(int shift)
        {
            if (_viewData.Selected == null) return;
            if (_viewData.Selected.Count() != 1) return;

            var before = _viewData.Selected.Unique;
            var after = before.Clone();

            var newTo = _viewData.Original.Callender.ApplyOffset(after.Period.To, shift);
            if (newTo == null) return;
            if (newTo < after.Period.From) return;

            after.Period.To = newTo;

            Replace(before, after);
            _viewData.Selected = new WorkItems(after);
        }
    }
}
