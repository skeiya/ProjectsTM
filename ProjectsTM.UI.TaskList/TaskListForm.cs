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
        private readonly TaskListGrid _gridControl;

        public TaskListForm(ViewData viewData, PatternHistory patternHistory, TaskListOption option)
        {
            InitializeComponent();
            _gridControl = new TaskListGrid(viewData);
            _gridControl.Dock = DockStyle.Fill;
            panel1.Controls.Add(_gridControl);
            InitializeCombobox(option.ErrorDisplayType, option);
            this._history = patternHistory;
            _gridControl.ListUpdated += GridControl1_ListUpdated;
            _gridControl.Option = option;
            this.Load += TaskListForm_Load;
            this.FormClosed += TaskListForm_FormClosed;
            this.checkBoxShowMS.CheckedChanged += CheckBoxShowMS_CheckedChanged;
            this.buttonEazyRegex.Click += buttonEazyRegex_Click;
            this.checkBoxShowMS.Checked = option.IsShowMS;
        }

        private void InitializeCombobox(ErrorDisplayType errorDisplayType, TaskListOption option)
        {
            comboBoxPattern.Text = option.Pattern;

            foreach (ErrorDisplayType d in Enum.GetValues(typeof(ErrorDisplayType)))
            {
                comboBoxErrorDisplay.Items.Add(GetString(d));
            }
            comboBoxErrorDisplay.SelectedIndex = (int)errorDisplayType;
            comboBoxErrorDisplay.SelectedIndexChanged += ComboBoxErrorDisplay_SelectedIndexChanged;
        }

        private void ComboBoxErrorDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            _gridControl.Option.ErrorDisplayType = (ErrorDisplayType)comboBoxErrorDisplay.SelectedIndex;
            UpdateList();
        }

        private static string GetString(ErrorDisplayType d)
        {
            switch (d)
            {
                case ErrorDisplayType.All:
                    return "すべて表示";
                case ErrorDisplayType.ErrorOnly:
                    return "エラーのみ";
                case ErrorDisplayType.OverlapOnly:
                    return "衝突のみ";
                default:
                    return string.Empty;
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
            return new TaskListOption(comboBoxPattern.Text, checkBoxShowMS.Checked, textBoxAndCondition.Text, _gridControl.Option.ErrorDisplayType);
        }

        private void TaskListForm_Load(object sender, EventArgs e)
        {
            this.Size = FormSizeRestoreService.LoadFormSize("TaskListFormSize");
            var colWidths = FormSizeRestoreService.LoadColWidths("TaskListColWidths");
            for (var idx = 0; idx < this._gridControl.ColWidths.Count; idx++)
            {
                if (colWidths == null || colWidths.Count() <= idx) break;
                this._gridControl.ColWidths[idx] = colWidths[idx];
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
            labelErrorCount.Text = "エラー数：" + _gridControl.GetErrorCount().ToString();
        }

        private void UpdateLabelSum()
        {
            var dayCount = _gridControl.GetDayCount();
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
            FormSizeRestoreService.SaveColWidths(this._gridControl.ColWidths.ToIntArray(), "TaskListColWidths");
        }

        private void SaveFormSize()
        {
            FormSizeRestoreService.SaveFormSize(Height, Width, "TaskListFormSize");
        }

        public void UpdateView()
        {
            _gridControl.UpdateView();
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
            _gridControl.Option = GetOption();
            _gridControl.UpdateView();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _gridControl.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
