using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement
{
    class WorkItems
    {

        private List<WorkItem> _items = new List<WorkItem>();
        private string _filter;

        internal void Add(WorkItem wi)
        {
            _items.Add(wi);
        }

        internal void SetFilter(string filter)
        {
            _filter = filter;
        }

        public List<WorkItem> GetWorkItems()
        {
            if (string.IsNullOrEmpty(_filter)) return _items;
            return _items.Where((i) => i.ToString().Equals(_filter)).ToList();
        }
    }
}
