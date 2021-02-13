﻿using ProjectsTM.Model;
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
        private readonly string _userName;

        public TaskListForm(ViewData viewData, PatternHistory patternHistory, TaskListOption option, string userName)
        {
            InitializeComponent();

            InitializeCombobox(option.ErrorDisplayType);
            this._history = patternHistory;
            gridControl1.ListUpdated += GridControl1_ListUpdated;
            gridControl1.Option = option;
            gridControl1.Initialize(viewData);
            this.Load += TaskListForm_Load;
            this.FormClosed += TaskListForm_FormClosed;
            this.checkBoxShowMS.CheckedChanged += CheckBoxShowMS_CheckedChanged;
            this.buttonEazyRegex.Click += buttonEazyRegex_Click;
            this.checkBoxShowMS.Checked = option.IsShowMS;
            this.comboBoxPattern.SelectedIndexChanged += ComboBoxPattern_SelectedIndexChanged;
            this._userName = userName;
        }

        private void InitializeCombobox(ErrorDisplayType errorDisplayType)
        {
            foreach (ErrorDisplayType d in Enum.GetValues(typeof(ErrorDisplayType)))
            {
                comboBoxErrorDisplay.Items.Add(GetString(d));
            }
            comboBoxErrorDisplay.SelectedIndex = (int)errorDisplayType;
            comboBoxErrorDisplay.SelectedIndexChanged += ComboBoxErrorDisplay_SelectedIndexChanged;
        }

        private void ComboBoxErrorDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridControl1.Option.ErrorDisplayType = (ErrorDisplayType)comboBoxErrorDisplay.SelectedIndex;
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
            if (comboBoxPattern.Text.Contains(_userName)) return GetSortPatternFormUserName(_userName);
            return new TaskListOption(comboBoxPattern.Text, checkBoxShowMS.Checked, textBoxAndCondition.Text, gridControl1.Option.ErrorDisplayType);
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
            SetUserNameSortSelect();
            comboBoxPattern.Items.AddRange(_history.Items.ToArray());
        }

        private void ComboBoxPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPattern.SelectedIndex != 0 || !IsUserSettingSet()) return;
            gridControl1.Option = GetSortPatternFormUserName(_userName);
            gridControl1.UpdateView();
        }

        private void SetUserNameSortSelect()
        {
            if (!IsUserSettingSet()) return;
            comboBoxPattern.Items.Add($"あなた({_userName})に割り当てられたタスク");
        }

        private TaskListOption GetSortPatternFormUserName(string userName)
        {
            if (userName.Contains("(")) userName = userName.Replace("(", @"\(");
            if (userName.Contains(")")) userName = userName.Replace(")", @"\)");
            return new TaskListOption(userName, false, string.Empty, gridControl1.Option.ErrorDisplayType);
        }

        private bool IsUserSettingSet()
        {
            return !string.IsNullOrEmpty(_userName);
        }

        private void buttonUpdate_Click(object sender, System.EventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            AppendSelectiontToHistory();
            gridControl1.Option = GetOption();
            gridControl1.UpdateView();
        }

        private void AppendSelectiontToHistory()
        {
            if (comboBoxPattern.Text.Equals($"あなた({_userName})に割り当てられたタスク")) return;
            _history.Append(comboBoxPattern.Text);
        }
    }
}
