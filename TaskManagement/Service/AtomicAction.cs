using System;
using System.Collections;
using System.Collections.Generic;

namespace TaskManagement.Service
{
    class AtomicAction : IEnumerable<EditAction>
    {
        private List<EditAction> _list = new List<EditAction>();

        public IEnumerator<EditAction> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        internal void Add(EditAction action)
        {
            _list.Add(action);
        }

        internal void Clear()
        {
            _list.Clear();
        }

        internal AtomicAction Clone()
        {
            var result = new AtomicAction();
            foreach (var i in _list) result.Add(i);
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
