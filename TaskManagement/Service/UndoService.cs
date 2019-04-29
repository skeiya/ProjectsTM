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
            if (IsSame(before, after)) return;
            _undoStack.Push(new Tuple<string, string>(before, after));
            _redoStack.Clear();
            Changed(this, null);
        }

        private static bool IsSame(string before, string after)
        {
            if (before == null && after == null) return true;
            if (before == null && after != null) return false;
            return before.Equals(after);
        }

        internal void Undo(WorkItems workItems)
        {
            if (_undoStack.Count == 0) return;
            var p = _undoStack.Pop();
            _redoStack.Push(p);
            var before = p.Item1 == null ? null : WorkItem.Deserialize(p.Item1);
            var after = p.Item2 == null ? null : WorkItem.Deserialize(p.Item2);
            if (after == null) // 削除のundo
            {
                workItems.Add(before);
            }
            else if (before == null) // 追加のundo
            {
                workItems.Remove(after);
            }
            else // 編集のundo
            {
                foreach (var w in workItems)
                {
                    if (w.Equals(after)) w.Apply(before);
                }
            }
            Changed(this, null);
        }

        internal void Redo(WorkItems workItems)
        {
            if (_redoStack.Count == 0) return;
            var r = _redoStack.Pop();
            _undoStack.Push(r);
            var before = r.Item1 == null ? null : WorkItem.Deserialize(r.Item1);
            var after = r.Item2 == null ? null : WorkItem.Deserialize(r.Item2);
            if (before == null)
            {
                workItems.Add(after);
            }
            else if (after == null)
            {
                workItems.Remove(before);
            }
            else
            {
                foreach (var w in workItems)
                {
                    if (w.Equals(before)) w.Apply(after);
                }
            }
            Changed(this, null);
        }
    }
}