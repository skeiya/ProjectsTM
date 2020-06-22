using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TaskManagement.Logic;
using TaskManagement.Model;
using TaskManagement.Service;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public partial class SearchWorkitemForm : Form
    {
        private readonly ViewData _viewData;
        private readonly WorkItemEditService _editService;
        private List<WorkItem> _list = new List<WorkItem>();

        public SearchWorkitemForm(ViewData viewData, WorkItemEditService editService)
        {
            InitializeComponent();
            this._viewData = viewData;
            this._editService = editService;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0) return;
            if (dataGridView1.RowCount - 1 <= dataGridView1.SelectedRows[0].Index) return;
            var index = GetListIndexSelected(dataGridView1.SelectedRows);
            if (index < 0) return;
            var found = _viewData.GetFilteredWorkItems().FirstOrDefault(w => w.Equals(_list[index]));
            _viewData.Selected = found == null ? null : new WorkItems(found);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || dataGridView1.ColumnCount - 1 <= e.ColumnIndex || e.RowIndex < 0 || dataGridView1.RowCount - 1 <= e.RowIndex) return;
            if (dataGridView1.SelectedRows.Count <= 0) return;
            var index = GetListIndexSelected(dataGridView1.SelectedRows);
            if (index < 0) return;
            var wi = _list[index];
            using (var dlg = new EditWorkItemForm(wi.Clone(), _viewData.Original.Callender))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                var newWi = dlg.GetWorkItem();
                _editService.Replace(wi, newWi);
                _viewData.Selected = new WorkItems(newWi);
                _viewData.UpdateCallenderAndMembers(wi);
                EditDataGridView(index, newWi);
            }
        }

        private void CheckBoxOverwrapPeriod_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPattern.Enabled = !checkBoxOverwrapPeriod.Checked;
            if (!checkBoxOverwrapPeriod.Checked)
            {
                return;
            }

            textBoxPattern.Text = string.Empty;
            _list = OverwrapedWorkItemsGetter.Get(_viewData.Original.WorkItems);
            UpdateDataGridView();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (IsSearchOverwrap())
            {
                CheckBoxOverwrapPeriod_CheckedChanged(null, null);
                return;
            }
            _list.Clear();
            try
            {
                foreach (var wi in _viewData.GetFilteredWorkItems())
                {
                    if (!Regex.IsMatch(wi.ToString(), textBoxPattern.Text, GetOption())) continue;
                    _list.Add(wi);
                }
            }
            catch { }
            UpdateDataGridView();
        }

        private RegexOptions GetOption()
        {
            if (checkBoxCaseDistinct.Checked) return RegexOptions.None;
            return RegexOptions.IgnoreCase;
        }

        private bool IsSearchOverwrap()
        {
            return checkBoxOverwrapPeriod.Checked;
        }      

        private void UpdateDataGridView()
        {
            dataGridView1.Rows.Clear();
            SetGridViewRows();
            var dayCount = _list.Sum(w => _viewData.Original.Callender.GetPeriodDayCount(w.Period));
            var monthCount = dayCount / 20;
            labelSum.Text = dayCount.ToString() + "day (" + monthCount.ToString() + "人月)";
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            var newSelect = new WorkItems();
            foreach(var w in _viewData.GetFilteredWorkItems())
            {
                if (_list.Any(i => i.Equals(w))) newSelect.Add(w);
            }
            _viewData.Selected = newSelect;
        }

        private void buttonEasyRegex_Click(object sender, EventArgs e)
        {
            using(var dlg = new EazyRegexForm())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                textBoxPattern.Text = dlg.RegexPattern;
            }
        }

        internal void Clear()
        {
            _list.Clear();
            UpdateDataGridView();
        }

         private void SearchWorkitemForm_Load(object sender, EventArgs e)
        {
            SetGridViewColumns();
        }
    }
}
