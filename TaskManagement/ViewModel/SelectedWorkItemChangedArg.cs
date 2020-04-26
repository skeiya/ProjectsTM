using System.Collections.Generic;
using TaskManagement.Model;

namespace TaskManagement.ViewModel
{
    public class SelectedWorkItemChangedArg
    {
        private WorkItem org;
        private WorkItem selected;

        public SelectedWorkItemChangedArg(WorkItem org, WorkItem selected)
        {
            this.org = org;
            this.selected = selected;
        }

        public List<Member> UpdatedMembers
        {
            get
            {
                var result = new List<Member>();
                if (org != null) result.Add(org.AssignedMember);
                if (selected != null) result.Add(selected.AssignedMember);
                return result;
            }
        }
    }
}