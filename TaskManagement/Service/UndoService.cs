using System;
using System.Collections.Generic;
using TaskManagement.Model;

namespace TaskManagement.Service
{
    public class UndoService
    {
        private Stack<AtomicAction> _undoStack = new Stack<AtomicAction>();
        private Stack<AtomicAction> _redoStack = new Stack<AtomicAction>();

        public event EventHandler Changed;

        public UndoService()
        {
        }

        private AtomicAction _atomicAction = new AtomicAction();

        internal void Delete(WorkItem w)
        {
            _atomicAction.Add(new EditAction(EditActionType.Delete, w.Serialize()));
        }

        internal void Add(WorkItem w)
        {
            _atomicAction.Add(new EditAction(EditActionType.Add, w.Serialize()));
        }

        internal void Push()
        {
            _undoStack.Push(_atomicAction.Clone());
            _atomicAction.Clear();
            _redoStack.Clear();
            Changed(this, null);
        }

        internal void Undo(WorkItems workItems)
        {
            if (_undoStack.Count == 0) return;
            var p = _undoStack.Pop();
            _redoStack.Push(p);
            foreach (var a in p)
            {
                var w = WorkItem.Deserialize(a.WorkItemText);
                if (a.Action == EditActionType.Add)
                {
                    workItems.Remove(w);
                }
                else if (a.Action == EditActionType.Delete)
                {
                    workItems.Add(w);
                }
            }
            Changed(this, null);
        }

        internal void Redo(WorkItems workItems)
        {
            if (_redoStack.Count == 0) return;
            var r = _redoStack.Pop();
            _undoStack.Push(r);
            foreach (var a in r)
            {
                var w = WorkItem.Deserialize(a.WorkItemText);
                if (a.Action == EditActionType.Add)
                {
                    workItems.Add(w);
                }
                else if (a.Action == EditActionType.Delete)
                {
                    workItems.Remove(w);
                }
            }
            Changed(this, null);
        }
    }
}