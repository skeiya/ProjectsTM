using ProjectsTM.Model;
using ProjectsTM.UI.TaskList;
using ProjectsTM.ViewModel;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    class TaskListManager
    {
        private readonly ViewData _viewData;
        private readonly PatternHistory _patternHistory;
        private readonly IWin32Window _parent;
        private bool _overWrapCheck;

        public TaskListManager(ViewData viewData, PatternHistory patternHistory, IWin32Window parent)
        {
            _viewData = viewData;
            _patternHistory = patternHistory;
            _parent = parent;
        }

        private TaskListForm TaskListForm { get; set; }

        internal void UpdateView()
        {
            if (TaskListForm != null && TaskListForm.Visible) TaskListForm.UpdateView();
        }

        internal void ShowOverWrapCheck()
        {
            Show(true);
        }

        internal void Show(bool overWrapCheck)
        {
            if (_overWrapCheck != overWrapCheck) TaskListForm?.Dispose();
            _overWrapCheck = overWrapCheck;

            if (TaskListForm == null || TaskListForm.IsDisposed)
            {
                TaskListForm = new TaskListForm(_viewData, _patternHistory, _overWrapCheck);
            }
            if (!TaskListForm.Visible) TaskListForm.Show(_parent);
        }
    }
}
