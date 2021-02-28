using ProjectsTM.Model;
using System;
using System.Collections.Generic;

namespace ProjectsTM.ViewModel
{
    public class UndoBuffer
    {
        private readonly Stack<AtomicAction> _undoStack = new Stack<AtomicAction>();
        private readonly Stack<AtomicAction> _redoStack = new Stack<AtomicAction>();
        private readonly AtomicAction _atomicAction = new AtomicAction();
        public event EventHandler<IEditedEventArgs> Changed;

        public void Push()
        {
            _undoStack.Push(_atomicAction.Clone());
            Changed?.Invoke(this, new EditedEventArgs(_atomicAction.Members));
            _atomicAction.Clear();
            _redoStack.Clear();
        }

        public void Undo(ViewData viewData)
        {
            if (_undoStack.Count == 0) return;
            var p = _undoStack.Pop();
            _redoStack.Push(p);
            var selected = new List<WorkItem>();
            foreach (var a in p)
            {
                var w = WorkItem.Deserialize(a.WorkItemText, a.Member);
                if (a.Action == EditActionType.Add)
                {
                    viewData.Original.WorkItems.Remove(w);
                }
                else if (a.Action == EditActionType.Delete)
                {
                    viewData.Original.WorkItems.Add(w);
                    selected.Add(w);
                }
            }
            viewData.Selected.Set(selected);
            Changed(this, new EditedEventArgs(p.Members));
        }

        public void Redo(ViewData viewData)
        {
            if (_redoStack.Count == 0) return;
            var r = _redoStack.Pop();
            _undoStack.Push(r);
            var selected = new List<WorkItem>();
            foreach (var a in r)
            {
                var w = WorkItem.Deserialize(a.WorkItemText, a.Member);
                if (a.Action == EditActionType.Add)
                {
                    viewData.Original.WorkItems.Add(w);
                    selected.Add(w);
                }
                else if (a.Action == EditActionType.Delete)
                {
                    viewData.Original.WorkItems.Remove(w);
                }
            }
            viewData.Selected.Set(selected);
            Changed(this, new EditedEventArgs(r.Members));
        }

        public void Clear()
        {
            _redoStack.Clear();
            _undoStack.Clear();
            _atomicAction.Clear();
        }

        public void Delete(IEnumerable<WorkItem> wis)
        {
            foreach (var w in wis) Delete(w);
        }

        public void Delete(WorkItem w)
        {
            _atomicAction.Add(new EditAction(EditActionType.Delete, w.Serialize(), w.AssignedMember));
        }

        public void Add(IEnumerable<WorkItem> wis)
        {
            foreach (var w in wis) Add(w);
        }

        public void Add(WorkItem w)
        {
            _atomicAction.Add(new EditAction(EditActionType.Add, w.Serialize(), w.AssignedMember));
        }
    }
}
