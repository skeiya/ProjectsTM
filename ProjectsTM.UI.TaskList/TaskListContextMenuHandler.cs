using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.TaskList
{
    class TaskListContextMenuHandler
    {
        private ViewData _viewData;
        private TaskListGrid _taskListGrid;

        public TaskListContextMenuHandler(ViewData viewData, TaskListGrid taskListGrid)
        {
            _viewData = viewData;
            _taskListGrid = taskListGrid;

            _taskListGrid.ContextMenuStrip = new ContextMenuStrip();
            _taskListGrid.ContextMenuStrip.Items.Add(new ToolStripMenuItem("→状態；Done", null, DoneMenu_Click, Keys.Control | Keys.D));
        }

        private void DoneMenu_Click(object sender, EventArgs e)
        {
            ChangeState(TaskState.Done);
        }

        private void ChangeState(TaskState state)
        {
            var selected = _viewData.Selected;
            if (selected == null) return;
            _taskListGrid.EditService.ChangeState(selected, state);
        }
    }
}
