using ProjectsTM.Model;
using System;
using System.Linq;

namespace ProjectsTM.Service
{
    public class WorkItemCopyPasteService
    {
        private WorkItem _copiedWorkItem;
        
        public void CopyWorkItem(WorkItems selected)
        {
            if (selected == null) return;
            if (selected.Count() != 1) return;
            _copiedWorkItem = selected.Unique.Clone();
        }

        public void PasteWorkItem(CallenderDay cursorDay, Member cursorMember, Callender callender, Action<WorkItem> Add)
        {
            if (_copiedWorkItem == null) return;
            if (cursorDay == null) return;
            if (cursorMember == null) return;

            var copyItem = _copiedWorkItem.Clone();
            var offset = callender.GetOffset(copyItem.Period.From, cursorDay);
            copyItem.Period = copyItem.Period.ApplyOffset(offset,callender);
            if (copyItem.Period == null) return;

            copyItem.AssignedMember = cursorMember;

            Add?.Invoke(copyItem);
        }
    }
}
