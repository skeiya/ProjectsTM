using System;
using System.Collections.Generic;

namespace TaskManagement.Service
{
    internal class UndoService
    {
        private Stack<Tuple<string, string>> _stack = new Stack<Tuple<string, string>>();

        public UndoService()
        {
        }

        internal void Push(string before, string after)
        {
            if (before.Equals(after)) return;
            _stack.Push(new Tuple<string, string>(before, after));
        }

        internal void Undo(WorkItems workItems)
        {
            if (_stack.Count == 0) return;
            var p = _stack.Pop();
            var before = WorkItem.Deserialize(p.Item1);
            var after = WorkItem.Deserialize(p.Item2);
            foreach (var w in workItems)
            {
                if (w.Equals(after)) w.Apply(before);
            }
        }
    }
}