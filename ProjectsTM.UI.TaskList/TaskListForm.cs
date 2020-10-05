﻿using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.UI.TaskList
{
    public partial class TaskListForm : Form
    {
        private readonly ViewData _viewData;
        private PatternHistory _history;
        private FormSize _formSize;

        public TaskListForm(ViewData viewData, PatternHistory patternHistory, FormSize formSize)
        {
            InitializeComponent();

            this._viewData = viewData;
            this._history = patternHistory;
            this._formSize = formSize;
            gridControl1.ListUpdated += GridControl1_ListUpdated;
            gridControl1.Initialize(viewData, comboBoxPattern.Text, _formSize.TaskListColWidths, checkBoxShowMS.Checked, textBoxAndCondition.Text);
            this.Size = FormSizeRestoreService.Load("TaskListFormSize");
            this.FormClosed += TaskListForm_FormClosed;
            this.checkBoxShowMS.CheckedChanged += CheckBoxShowMS_CheckedChanged;
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
            _formSize.TaskListColWidths.Clear();
            for (var idx = 0; idx < gridControl1.ColCount; idx++)
            {
                _formSize.TaskListColWidths.Add(gridControl1.ColWidths[idx]);
            }
            FormSizeRestoreService.Save(Height, Width, "TaskListFormSize");
        }

        public void Clear()
        {
            gridControl1.Initialize(_viewData, comboBoxPattern.Text, _formSize.TaskListColWidths, checkBoxShowMS.Checked, textBoxAndCondition.Text);
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
            gridControl1.Initialize(_viewData, comboBoxPattern.Text, _formSize.TaskListColWidths, checkBoxShowMS.Checked, textBoxAndCondition.Text);
        }
    }
}
