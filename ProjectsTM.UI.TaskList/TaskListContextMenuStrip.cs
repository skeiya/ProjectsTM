using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.TaskList
{
    class TaskListContextMenuStrip : ContextMenuStrip
    {
        private readonly ViewData _viewData;
        private readonly TaskListGrid _grid;

        public TaskListContextMenuStrip(ViewData viewData, TaskListGrid grid)
        {
            this._viewData = viewData;
            this._grid = grid;

            this.Items.Add(new ToolStripMenuItem("→状態；Done", null, DoneMenu_Click, Keys.Control | Keys.D));
        }

        private void DoneMenu_Click(object sender, EventArgs e)
        {
            var selected = _viewData.Selected;
            _grid.EditService.ChangeState(selected, TaskState.Done);
        }
    }
}
