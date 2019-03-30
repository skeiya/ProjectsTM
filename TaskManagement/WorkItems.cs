using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement
{
    class WorkItems : IEnumerable<WorkItem>
    {
        private List<WorkItem> _items = new List<WorkItem>();

        internal void Add(WorkItem wi)
        {
            _items.Add(wi);
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

        public IEnumerator<WorkItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
