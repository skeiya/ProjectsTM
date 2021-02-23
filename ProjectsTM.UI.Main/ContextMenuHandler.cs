using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    internal class ContextMenuHandler
    {
        private readonly ViewData _viewData;
        private readonly WorkItemGrid _grid;

        public ContextMenuHandler(ViewData viewData, WorkItemGrid grid)
        {
            if (viewData is null) throw new ArgumentNullException(nameof(viewData));
            if (grid is null) throw new ArgumentNullException(nameof(grid));

            _viewData = viewData;
            _grid = grid;
        }

        public void Initialize(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip is null) throw new ArgumentNullException(nameof(contextMenuStrip));

            contextMenuStrip.Items.Add(new ToolStripMenuItem("編集(&E)...", null, EditMenu_Click, Keys.Control | Keys.E));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("コピー(&C)", null, CopyMenu_Click, Keys.Control | Keys.C));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("貼り付け(&P)", null, PasteMenu_Click, Keys.Control | Keys.V));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("削除(&D)", null, DeleteMenu_Click, Keys.Delete));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("分割(&I)...", null, DivideMenu_Click, Keys.Control | Keys.I));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("今日にジャンプ(&T)", null, JumpTodayMenu_Click, Keys.Control | Keys.T));
            contextMenuStrip.Items.Add(new ToolStripMenuItem("→状態；Done", null, DoneMenu_Click, Keys.Control | Keys.D));
            var manageItem = new ToolStripMenuItem("管理用(&M)");
            contextMenuStrip.Items.Add(manageItem);
            manageItem.DropDownItems.Add(new ToolStripMenuItem("&2分割", null, DivideInto2PartsMenu_Click, Keys.Control | Keys.D2));
            manageItem.DropDownItems.Add(new ToolStripMenuItem("半分に縮小(&H)", null, MakeHalfMenu_Click, Keys.Control | Keys.H));
            manageItem.DropDownItems.Add("以降を選択").Click += SelectAfterwardMenu_Click;
            manageItem.DropDownItems.Add("以降を前詰めに整列").Click += AlignAfterwardMenu_Click;
            manageItem.DropDownItems.Add("選択中の作業項目を隙間なく並べる").Click += AlignSelectedMenu_Click;
            manageItem.DropDownItems.Add("→状態：Background").Click += BackgroundMenu_Click;
        }

        private void PasteMenu_Click(object sender, EventArgs e)
        {
            _grid.PasteWorkItem();
        }

        private void CopyMenu_Click(object sender, EventArgs e)
        {
            _grid.CopyWorkItem();
        }

        private void BackgroundMenu_Click(object sender, EventArgs e)
        {
            ChangeState(TaskState.Background);
        }

        private void EditMenu_Click(object sender, EventArgs e)
        {
            _grid.EditSelectedWorkItem();
        }


        private void DeleteMenu_Click(object sender, EventArgs e)
        {
            _grid.EditService.Delete();
        }

        private void AlignSelectedMenu_Click(object sender, EventArgs e)
        {
            _grid.EditService.AlignSelected();
        }

        private void MakeHalfMenu_Click(object sender, EventArgs e)
        {
            _grid.EditService.MakeHalf();
        }

        private void DivideInto2PartsMenu_Click(object sender, EventArgs e)
        {
            _grid.EditService.DivideInto2Parts();
        }

        private void SelectAfterwardMenu_Click(object sender, EventArgs e)
        {
            var selected = _viewData.Selected;
            if (selected == null) return;
            _grid.EditService.SelectAfterward(selected);
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

        private void DivideMenu_Click(object sender, EventArgs e)
        {
            _grid.Divide();
        }
        private void JumpTodayMenu_Click(object sender, EventArgs e)
        {
            _grid.MoveToToday();
        }

        private void AlignAfterwardMenu_Click(object sender, EventArgs e)
        {
            if (!_grid.EditService.AlignAfterward())
            {
                MessageBox.Show(_grid, "期間を正常に更新できませんでした。");
            }
        }
    }
}
