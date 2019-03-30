using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
                filteredList = _items.Where((i) => Regex.IsMatch(i.ToString(), _filter)).ToList();
            }
            else
            {
                filteredList = _items;
            }
            if (members != null)
            {
                filteredList = filteredList.Where((i) => members.Contains(i.AssignedMember)).ToList();
            }
            if (days != null)
            {
                filteredList = filteredList.Where((i) => days.Contains(i.Period.From) && days.Contains(i.Period.To)).ToList();
            }
            return filteredList;
        }

        internal void SetFilter(object workItem)
        {
            throw new NotImplementedException();
        }

        internal int GetWorkItemDaysOfMonth(int year, int month, Member member, Project project)
        {
            int result = 0;
            foreach (var wi in _items.Where((w) => w.AssignedMember.Equals(member) && w.Project.Equals(project)))
            {
                foreach (var d in wi.Period.Days)
                {
                    if (d.Year != year) continue;
                    if (d.Month != month) continue;
                    result++;
                }
            }
            return result;
        }
    }
}
