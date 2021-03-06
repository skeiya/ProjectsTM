﻿using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Linq;

namespace ProjectsTM.Service
{
    public class WorkItemCopyPasteService
    {
        private WorkItem _copiedWorkItem;

        public void CopyWorkItem(SelectedWorkItems selected)
        {
            if (selected.Count() != 1) return;
            _copiedWorkItem = selected.Unique.Clone();
        }

        public void PasteWorkItem(CallenderDay cursorDay, Member cursorMember, WorkItemEditService editor)
        {
            editor.CopyAndAdd(_copiedWorkItem, cursorDay, cursorMember);
        }
    }
}
