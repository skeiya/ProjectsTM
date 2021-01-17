using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.TaskList
{
    internal class ContextMenuHandler
    {
        private readonly ViewData _viewData;
        private readonly TaskListGrid _grid;

        internal ContextMenuHandler(ViewData viewData, TaskListGrid grid)
        {
            if (viewData is null) throw new ArgumentNullException(nameof(viewData));
            if (grid is null) throw new ArgumentNullException(nameof(grid));

            _viewData = viewData;
            _grid = grid;
        }

        internal void Initialize(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip is null) throw new ArgumentNullException(nameof(contextMenuStrip));

            contextMenuStrip.Items.Add(new ToolStripMenuItem("→状態；Done", null, DoneMenu_Click, Keys.Control | Keys.D));
        }

        private void DoneMenu_Click(object sender, EventArgs e)
        {
            ChangeState(TaskState.Done);
        }

        private void ChangeState(TaskState state)
        {
            var selected = _viewData.Selected;
            if (selected == null) return;
            _grid.EditService.ChangeState(selected, state);
        }
    }
}
