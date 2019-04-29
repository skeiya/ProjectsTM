using System.Linq;
using TaskManagement.Model;

namespace TaskManagement.Service
{
    class WorkItemEditService
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
                _undoService.Push(null, item.Serialize());
            }
        }

        internal void Delete(WorkItem selected)
        {
            _viewData.Original.WorkItems.Remove(_viewData.Selected);
            _undoService.Push(_viewData.Selected.Serialize(), null);
        }

        internal void Devide(WorkItem selected, int devided, int remain)
        {
            var d1 = selected.Clone();
            var d2 = selected.Clone();

            d1.Period.To = _viewData.Original.Callender.ApplyOffset(d1.Period.To, -remain);
            d2.Period.From = _viewData.Original.Callender.ApplyOffset(d2.Period.From, devided);

            var workItems = _viewData.Original.WorkItems;
            _viewData.Selected = null;
            workItems.Remove(selected);
            workItems.Add(d1);
            workItems.Add(d2);
        }
    }
}
