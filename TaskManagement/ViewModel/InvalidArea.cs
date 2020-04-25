using System.Collections.Generic;
using TaskManagement.Model;

namespace TaskManagement.ViewModel
{
    class InvalidArea
    {
        private HashSet<WorkItem> _validList = new HashSet<WorkItem>();
        internal void Validate(WorkItem wi)
        {
            if (IsValid(wi)) return;
            _validList.Add(wi);
        }

        internal bool IsValid(WorkItem wi)
        {
            return _validList.Contains(wi);
        }
    }
}
