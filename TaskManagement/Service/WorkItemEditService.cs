using System.Linq;
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

        internal void Devide(WorkItem selected, int devided, int remain)
        {
            var d1 = selected.Clone();
            var d2 = selected.Clone();

            d1.Period.To = _viewData.Original.Callender.ApplyOffset(d1.Period.To, -remain);
            d2.Period.From = _viewData.Original.Callender.ApplyOffset(d2.Period.From, devided);

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

            foreach(var w in done) w.State = TaskState.Done;

            _undoService.Delete(selected);
            _undoService.Add(done);
            _undoService.Push();

            var workItems = _viewData.Original.WorkItems;
            _viewData.Selected = null;
            workItems.Remove(selected);
            workItems.Add(done);
        }
    }
}
