﻿using ProjectsTM.Model;
using System.Collections.Generic;
using System.Linq;

namespace ProjectsTM.ViewModel
{
    public class SelectedWorkItemChangedArg
    {
        private readonly WorkItems org;
        private readonly WorkItems selected;

        public SelectedWorkItemChangedArg(WorkItems org, WorkItems selected)
        {
            this.org = org;
            this.selected = selected;
        }

        public IEnumerable<Member> UpdatedMembers
        {
            get
            {
                var result = new List<Member>();
                if (org != null) result.AddRange(org.Select(w => w.AssignedMember));
                if (selected != null) result.AddRange(selected.Select(w => w.AssignedMember));
                return result.Distinct();
            }
        }
    }
}