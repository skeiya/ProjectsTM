using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement.Model
{
    public class WorkItems : IEnumerable<WorkItem>
    {

        private SortedDictionary<Member, MembersWorkItems> _items = new SortedDictionary<Member, MembersWorkItems>();

        public IEnumerable<MembersWorkItems> EachMembers => _items.Values;
        public MembersWorkItems OfMember(Member m) => _items.ContainsKey(m) ? _items[m] : new MembersWorkItems();

        public void Add(WorkItem wi)
        {
            if (!_items.ContainsKey(wi.AssignedMember))
            {
                _items.Add(wi.AssignedMember, new MembersWorkItems());
            }
            _items[wi.AssignedMember].Add(wi);
        }

        public int GetWorkItemDaysOfMonth(int year, int month, Member member, Project project, Callender callender)
        {
            int result = 0;
            foreach (var wi in this.Where((w) => w.AssignedMember.Equals(member) && w.Project.Equals(project)))
            {
                foreach (var d in callender.GetPediodDays(wi.Period))
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
            return _items.SelectMany((s) => s.Value).GetEnumerator();
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

        internal void Remove(WorkItem selected)
        {
            if (!_items[selected.AssignedMember].Remove(selected))
            {
                throw new System.Exception();
            }
        }

        public override int GetHashCode()
        {
            return -566117206 + EqualityComparer<SortedDictionary<Member, MembersWorkItems>>.Default.GetHashCode(_items);
        }
    }
}
