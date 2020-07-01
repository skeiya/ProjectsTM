using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace TaskManagement.Model
{
    public class MembersWorkItems : IEnumerable<WorkItem>
    {
        private List<WorkItem> _items = new List<WorkItem>();
        private int _sumCache = 0;
        public int Sum => _sumCache;
        public int Count => _items.Count;

        public int CountInPeriod(Period period)
        {
            if (period == null) return Count;
            Debug.Assert((period.From != null) && (period.To != null));

            int countInPeriod = 0;
            foreach (var w in _items)
            {
                if (period.Contains(w.Period.From) ||
                    period.Contains(w.Period.To)) countInPeriod++;
            }
            return countInPeriod;
        }

        public bool IsNoWorkItem(Period period)
        {
            return CountInPeriod(period) == 0;
        }

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
