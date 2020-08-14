using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProjectsTM.Model
{
    public class WorkItems : IEnumerable<WorkItem>
    {
        public WorkItems() { }
        public WorkItems(WorkItem w)
        {
            Add(w);
        }

        public WorkItems(WorkItems ws)
        {
            this._items = new SortedDictionary<Member, MembersWorkItems>(ws._items);
        }

        public WorkItems(IEnumerable<WorkItem> wis)
        {
            foreach (var w in wis) Add(w);
        }

        private SortedDictionary<Member, MembersWorkItems> _items = new SortedDictionary<Member, MembersWorkItems>();

        public IEnumerable<MembersWorkItems> EachMembers => _items.Values;

        public WorkItem Unique
        {
            get
            {
                Debug.Assert(this.Count() == 1);
                return _items.First().Value.First();
            }
        }

        public MembersWorkItems OfMember(Member m) => _items.ContainsKey(m) ? _items[m] : new MembersWorkItems();

        public void Add(WorkItems wis)
        {
            foreach (var wi in wis) Add(wi);
        }

        public void Add(WorkItem wi)
        {
            if (!_items.ContainsKey(wi.AssignedMember))
            {
                _items.Add(wi.AssignedMember, new MembersWorkItems());
            }
            _items[wi.AssignedMember].Add(wi);
        }

        public int GetWorkItemDaysOfGetsudo(int year, int month, Member member, Project project, Callender callender)
        {
            int result = 0;
            foreach (var wi in this.Where((w) => w.AssignedMember.Equals(member) && w.Project.Equals(project)))
            {
                foreach (var d in callender.GetPediodDays(wi.Period))
                {
                    if (!Callender.IsSameGetsudo(d, year, month)) continue;
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

        public void Remove(WorkItems selected)
        {
            foreach (var wi in selected) Remove(wi);
        }

        public void Remove(WorkItem selected)
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

        public WorkItems Clone()
        {
            var result = new WorkItems();
            foreach (var ws in this._items)
            {
                foreach (var w in ws.Value)
                {
                    result.Add(w.Clone());
                }
            }
            return result;
        }
    }
}
