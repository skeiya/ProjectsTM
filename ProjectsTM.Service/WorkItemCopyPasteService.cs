using ProjectsTM.Model;
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

        public void PasteWorkItem(CallenderDay cursorDay, Member cursorMember, WorkItemEditService editor)
        {
            editor.CopyAndAdd(_copiedWorkItem, cursorDay, cursorMember);
        }
    }
}
