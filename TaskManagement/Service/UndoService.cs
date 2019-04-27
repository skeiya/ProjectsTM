using System;
using System.Collections.Generic;

namespace TaskManagement.Service
{
    public class UndoService
    {
        private Stack<Tuple<string, string>> _undoStack = new Stack<Tuple<string, string>>();
        private Stack<Tuple<string, string>> _redoStack = new Stack<Tuple<string, string>>();

        public event EventHandler Changed;

        public UndoService()
        {
        }

        internal void Push(string before, string after)
        {
            if (before.Equals(after)) return;
            _undoStack.Push(new Tuple<string, string>(before, after));
            _redoStack.Clear();
            Changed(this, null);
        }

        internal void Undo(WorkItems workItems)
        {
            if (_undoStack.Count == 0) return;
            var p = _undoStack.Pop();
            _redoStack.Push(p);
            var before = WorkItem.Deserialize(p.Item1);
            var after = WorkItem.Deserialize(p.Item2);
            foreach (var w in workItems)
            {
                if (w.Equals(after)) w.Apply(before);
            }
            Changed(this, null);
        }

        internal void Redo(WorkItems workItems)
        {
            if (_redoStack.Count == 0) return;
            var r =_redoStack.Pop();
            _undoStack.Push(r);
            var before = WorkItem.Deserialize(r.Item1);
            var after = WorkItem.Deserialize(r.Item2);
            foreach (var w in workItems)
            {
                if (w.Equals(before)) w.Apply(after);
            }
            Changed(this, null);
        }
    }
}