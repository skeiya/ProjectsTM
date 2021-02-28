using ProjectsTM.Model;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.ViewModel
{
    public class SelectedWorkItemChangedArg
    {
        private readonly IEnumerable<WorkItem> _before;
        private readonly IEnumerable<WorkItem> _after;

        public SelectedWorkItemChangedArg(IEnumerable<WorkItem> before, IEnumerable<WorkItem> after)
        {
            this._before = before;
            this._after = after;
        }

        public IEnumerable<Member> UpdatedMembers
        {
            get
            {
                var result = new List<Member>();
                if (_before != null) result.AddRange(_before.Select(w => w.AssignedMember));
                if (_after != null) result.AddRange(_after.Select(w => w.AssignedMember));
                return result.Distinct();
            }
        }
    }
}