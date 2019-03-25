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

        public List<WorkItem> GetWorkItems(Members members, List<CallenderDay> days)
        {
            var filteredList = new List<WorkItem>();
            if (!string.IsNullOrEmpty(_filter))
            {
                filteredList = _items.Where((i) => i.ToString().Equals(_filter)).ToList();
            }
            {
                filteredList = _items;
            }
            if (members != null)
            {
                filteredList = filteredList.Where((i) => members.Contains(i.AssignedMember)).ToList();
            }
            if(days != null)
            {
                filteredList = filteredList.Where((i) => days.Contains(i.Period.From) && days.Contains(i.Period.To)).ToList();
            }
            return filteredList;
        }
    }
}
