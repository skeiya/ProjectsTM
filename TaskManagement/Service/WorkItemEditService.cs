using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
