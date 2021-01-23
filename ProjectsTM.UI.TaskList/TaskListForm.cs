using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.Common;
using ProjectsTM.ViewModel;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.UI.TaskList
{
    public partial class TaskListForm : Form
    {
        private readonly PatternHistory _history;
        private readonly bool _overWrapCheck;

        public TaskListForm(ViewData viewData, PatternHistory patternHistory, bool overWrapCheck)
        {
            InitializeComponent();

            this._history = patternHistory;
            this._overWrapCheck = overWrapCheck;
            gridControl1.ListUpdated += GridControl1_ListUpdated;
            gridControl1.Option = GetOption();
            gridControl1.Initialize(viewData);
            this.Load += TaskListForm_Load;
            this.FormClosed += TaskListForm_FormClosed;
            this.checkBoxShowMS.CheckedChanged += CheckBoxShowMS_CheckedChanged;
            this.buttonEazyRegex.Click += buttonEazyRegex_Click;
            if (_overWrapCheck)
            {
                this.Text = "衝突チェック";
                this.checkBoxShowMS.Checked = false;
            }
        }

        private void buttonEazyRegex_Click(object sender, EventArgs e)
        {
            using (var dlg = new EazyRegexForm())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                comboBoxPattern.Text = dlg.RegexPattern;
            }
        }

        private TaskListOption GetOption()
        {
            return new TaskListOption(comboBoxPattern.Text, checkBoxShowMS.Checked, textBoxAndCondition.Text, _overWrapCheck);
        }

        private void TaskListForm_Load(object sender, EventArgs e)
        {
            this.Size = FormSizeRestoreService.LoadFormSize("TaskListFormSize");
            var colWidths = FormSizeRestoreService.LoadColWidths("TaskListColWidths");
            for (var idx = 0; idx < this.gridControl1.ColWidths.Count; idx++)
            {
                if (colWidths == null || colWidths.Count() <= idx) break;
                this.gridControl1.ColWidths[idx] = colWidths[idx];
            }
        }

        private void CheckBoxShowMS_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateList();
        }

        private void GridControl1_ListUpdated(object sender, System.EventArgs e)
        {
            UpdateLabelSum();
            UpdateErrorCount();
        }

        private void UpdateErrorCount()
        {
            labelErrorCount.Text = "エラー数：" + gridControl1.GetErrorCount().ToString();
        }

        private void UpdateLabelSum()
        {
            var dayCount = gridControl1.GetDayCount();
            var monthCount = (dayCount / 20f);
            labelSum.Text = string.Format("{0}day {1:0.0}人月 ", dayCount, monthCount);
        }

        private void TaskListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveFormSize();
            SaveColWidths();
        }

        private void SaveColWidths()
        {
            FormSizeRestoreService.SaveColWidths(this.gridControl1.ColWidths.ToIntArray(), "TaskListColWidths");
        }

        private void SaveFormSize()
        {
            FormSizeRestoreService.SaveFormSize(Height, Width, "TaskListFormSize");
        }

        public void UpdateView()
        {
            gridControl1.UpdateView();
        }

        private void comboBoxPattern_DropDown(object sender, System.EventArgs e)
        {
            comboBoxPattern.Items.Clear();
            comboBoxPattern.Items.AddRange(_history.Items.ToArray());
        }
        private void buttonUpdate_Click(object sender, System.EventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            _history.Append(comboBoxPattern.Text);
            gridControl1.Option = GetOption();
            gridControl1.UpdateView();
        }
    }
}
