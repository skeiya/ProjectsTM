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

        public void Add(WorkItem item)
        {
            var items = _viewData.Original.WorkItems;
            if (item != null && !items.Contains(item))
            {
                items.Add(item);
                _undoService.Add(item);
                _undoService.Push();
            }
        }

        internal void Delete(WorkItem selected)
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

        internal void Replace(WorkItem before, WorkItem after)
        {
            if (before.Equals(after)) return;
            _viewData.Original.WorkItems.Remove(before);
            _viewData.Original.WorkItems.Add(after);
            _undoService.Delete(before);
            _undoService.Add(after);
            _undoService.Push();
        }

        internal void Done(WorkItem selected)
        {
            var done = selected.Clone();

            done.State = TaskState.Done;

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
