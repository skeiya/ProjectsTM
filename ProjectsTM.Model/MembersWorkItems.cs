using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProjectsTM.Model
{
    public class MembersWorkItems : IEnumerable<WorkItem>
    {
        private List<WorkItem> _items = new List<WorkItem>();
        private int _sumCache = 0;
        public int Sum => _sumCache;
        public int Count => _items.Count;

        public bool HasWorkItem(Period period)
        {
            if (period == null) return Count != 0;
            Debug.Assert((period.From != null) && (period.To != null));
            foreach (var w in _items)
            {
                if (((period.From <= w.Period.From) && (w.Period.From <= period.To)) ||
                   ((period.From <= w.Period.To) && (w.Period.To <= period.To))) return true;
            }
            return false;
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

        public void Add(WorkItem wi)
        {
            _items.Add(wi);
        }

        public bool Remove(WorkItem selected)
        {
            return _items.Remove(selected);
        }

        public List<Project> GetProjects()
        {
            var result = new List<Project>();
            foreach(var w in _items)
            {
                if (!result.Contains(w.Project)) result.Add(w.Project);
            }
            return result;
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

        public void SortByPeriodStartDate()
        {
            _items.Sort((a, b) => a.Period.From.CompareTo(b.Period.From));
        }
    }
}
