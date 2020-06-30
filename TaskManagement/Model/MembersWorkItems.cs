using System.Collections;
using System.Collections.Generic;

namespace TaskManagement.Model
{
    public class MembersWorkItems : IEnumerable<WorkItem>
    {
        private List<WorkItem> _items = new List<WorkItem>();
        private int _sumCache = 0;
        public int Sum => _sumCache;
        public int Count => _items.Count;

        public MembersWorkItems()
        {
        }

        public IEnumerator<WorkItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        internal void Add(WorkItem wi)
        {
            _items.Add(wi);
        }

        internal bool Remove(WorkItem selected)
        {
            return _items.Remove(selected);
        }

        public override bool Equals(object obj)
        {
            var items = obj as MembersWorkItems;
            if (items == null) return false;
            if (_items.Count != items._items.Count) return false;
            for (var index = 0; index < _items.Count; index++)
            {
                if (!_items[index].Equals(items._items[index])) return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return -566117206 + EqualityComparer<List<WorkItem>>.Default.GetHashCode(_items);
        }
    }
}
