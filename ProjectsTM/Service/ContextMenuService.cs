using System;
using System.Windows.Forms;
using ProjectsTM.Model;
using ProjectsTM.UI;
using ProjectsTM.ViewModel;

namespace ProjectsTM.Service
{
    class ContextMenuService
    {
        private ViewData _viewData;
        private WorkItemGrid workItemGrid1;

        public ContextMenuService(ViewData viewData, WorkItemGrid workItemGrid1)
        {
            _viewData = viewData;
            this.workItemGrid1 = workItemGrid1;


            workItemGrid1.ContextMenuStrip = new ContextMenuStrip();
            workItemGrid1.ContextMenuStrip.Items.Add("編集...").Click += EditMenu_Click;
            workItemGrid1.ContextMenuStrip.Items.Add("削除").Click += DeleteMenu_Click;
            workItemGrid1.ContextMenuStrip.Items.Add("分割...").Click += DivideMenu_Click;
            workItemGrid1.ContextMenuStrip.Items.Add("今日にジャンプ").Click += JumpTodayMenu_Click;
            workItemGrid1.ContextMenuStrip.Items.Add("→状態；Done").Click += DoneMenu_Click;
            var manageItem = new ToolStripMenuItem("管理用");
            workItemGrid1.ContextMenuStrip.Items.Add(manageItem);
            manageItem.DropDownItems.Add("2分割").Click += DivideInto2PartsMenu_Click;
            manageItem.DropDownItems.Add("半分に縮小").Click += MakeHalfMenu_Click;
            manageItem.DropDownItems.Add("以降を選択").Click += SelectAfterwardMenu_Click;
            manageItem.DropDownItems.Add("以降を前詰めに整列").Click += AlignAfterwardMenu_Click;
            manageItem.DropDownItems.Add("選択中の作業項目を隙間なく並べる").Click += AlignSelectedMenu_Click;
            manageItem.DropDownItems.Add("→状態：Background").Click += BackgroundMenu_Click;
        }

        private void BackgroundMenu_Click(object sender, EventArgs e)
        {
            ChangeState(TaskState.Background);
        }

        private void EditMenu_Click(object sender, EventArgs e)
        {
            workItemGrid1.EditSelectedWorkItem();
        }


        private void DeleteMenu_Click(object sender, EventArgs e)
        {
            workItemGrid1.EditService.Delete();
        }

        private void AlignSelectedMenu_Click(object sender, EventArgs e)
        {
            workItemGrid1.EditService.AlignSelected();
        }

        private void MakeHalfMenu_Click(object sender, EventArgs e)
        {
            workItemGrid1.EditService.MakeHalf();
        }

        private void DivideInto2PartsMenu_Click(object sender, EventArgs e)
        {
            workItemGrid1.EditService.DivideInto2Parts();
        }

        private void SelectAfterwardMenu_Click(object sender, EventArgs e)
        {
            var selected = _viewData.Selected;
            if (selected == null) return;
            workItemGrid1.EditService.SelectAfterward(selected);
        }

        private void DoneMenu_Click(object sender, EventArgs e)
        {
            ChangeState(TaskState.Done);
        }

        private void ChangeState(TaskState state)
        {
            var selected = _viewData.Selected;
            if (selected == null) return;
            workItemGrid1.EditService.ChangeState(selected, state);
        }

        private void DivideMenu_Click(object sender, EventArgs e)
        {
            workItemGrid1.Divide();
        }
        private void JumpTodayMenu_Click(object sender, EventArgs e)
        {
            workItemGrid1.MoveToToday();
        }

        private void AlignAfterwardMenu_Click(object sender, EventArgs e)
        {
            if (!workItemGrid1.EditService.AlignAfterward())
            {
                MessageBox.Show(workItemGrid1, "期間を正常に更新できませんでした。");
            }
        }
    }
}
