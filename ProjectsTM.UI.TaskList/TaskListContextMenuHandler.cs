using ProjectsTM.Model;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.TaskList
{
    public partial class TaskListGrid
    {
        private partial class ContextMenuHandler
        {
            private readonly TaskListGrid _grid;

            internal ContextMenuHandler(TaskListGrid grid)
            {
                _grid = grid;
            }

            internal void initializeContextMenu(ContextMenuStrip contextMenuStrip)
            {
                if (contextMenuStrip is null) throw new ArgumentNullException();
                if (_grid._viewData is null || _grid._editService is null)
                {
                    throw new InvalidOperationException("TaskListGridの中身が不完全な状態で呼び出された");
                }

                contextMenuStrip.Items.Add(new ToolStripMenuItem("→状態；Done", null, DoneMenu_Click, Keys.Control | Keys.D));
            }

            private void DoneMenu_Click(object sender, EventArgs e)
            {
                ChangeState(TaskState.Done);
            }

            private void ChangeState(TaskState state)
            {
                var selected = _grid._viewData.Selected;
                if (selected == null) return;
                _grid._editService.ChangeState(selected, state);
            }
        }
    }
}
