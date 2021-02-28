using ProjectsTM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsTM.ViewModel
{
    public class SelectedWorkItems : IEnumerable<WorkItem>
    {
        private WorkItems _workItems = new WorkItems();

        public event EventHandler<SelectedWorkItemChangedArg> Changed;

        public void Clear()
        {
            _workItems.Clear();
        }

        public bool ContainsDay(CallenderDay d)
        {
            return _workItems.Any(w => w.Period.Contains(d));
        }

        public bool ContainsMember(Member m)
        {
            return _workItems.Any(w => w.AssignedMember.Equals(m));
        }

        public void Set(IEnumerable<WorkItem> wis)
        {
            var earg = new SelectedWorkItemChangedArg(_workItems, wis);
            _workItems = new WorkItems(wis);
            Changed?.Invoke(this, earg);
        }

        public bool IsSameName(WorkItem wi)
        {
            if (_workItems.Count() != 1) return false;
            if (_workItems.Unique.Equals(wi)) return false;
            return _workItems.Unique.Name == wi.Name;
        }

        //public bool Contains(WorkItem wi)
        //{
        //    return _workItems.Contains(wi);
        //}

        //public int Count()
        //{
        //    return _workItems.Count();
        //}

        //public WorkItem ElementAt(int i)
        //{
        //    return _workItems.ElementAt(i);
        //}

        public IEnumerator<WorkItem> GetEnumerator()
        {
            return _workItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _workItems.GetEnumerator();
        }

        public WorkItem Unique => _workItems.Unique;

        public bool IsEmpty()
        {
            return !_workItems.Any();
        }

        public void Add(WorkItem wi)
        {
            _workItems.Add(wi);
        }

        public void Remove(WorkItem wi)
        {
            _workItems.Remove(wi);
        }

        public SelectedWorkItems Clone()
        {
            var result = new SelectedWorkItems();
            foreach (var w in _workItems)
            {
                result._workItems.Add(w.Clone());
            }
            return result;
        }
    }
}
