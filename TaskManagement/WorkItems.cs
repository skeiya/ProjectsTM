using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement
{
    public class WorkItems : IEnumerable<WorkItem>
    {
        private List<WorkItem> _items = new List<WorkItem>();

        public void Add(WorkItem wi)
        {
            _items.Add(wi);
        }

        public int GetWorkItemDaysOfMonth(int year, int month, Member member, Project project)
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

        public override bool Equals(object obj)
        {
            var target = obj as WorkItems;
            if (target == null) return false;
            if (_items.Count != target._items.Count) return false;
            return _items.SequenceEqual(target._items);
        }
    }
}
