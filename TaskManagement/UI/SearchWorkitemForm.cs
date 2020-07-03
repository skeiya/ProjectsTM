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
                if (checkBoxIncludeMilestone.Checked)
                {
                    foreach (var ms in _viewData.Original.MileStones)
                    {
                        var w = new WorkItem(
                            new Project("noProj"),
                            "↑" + ms.Name,
                            new Tags(new List<string>() { "" }),
                            new Period(ms.Day, ms.Day),
                            new Member(string.Empty, string.Empty, string.Empty),
                            TaskState.Active,
                            string.Empty
                            );
                        _list.Add(w);
                    }
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
            foreach (var w in _viewData.GetFilteredWorkItems())
            {
                if (_list.Any(i => i.Equals(w))) newSelect.Add(w);
            }
            _viewData.Selected = newSelect;
        }

        private void buttonEasyRegex_Click(object sender, EventArgs e)
        {
            using (var dlg = new EazyRegexForm())
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

        private enum GridCols
        {
            Name = 0,
            Proj,
            Assigned,
            Tag,
            State,
            From,
            To,
            Days,
            Description,
            Count,
        }

        private void SetGridViewColumns()
        {
            dataGridView1.ColumnCount = (int)GridCols.Count;
            dataGridView1.Columns[(int)GridCols.Name].HeaderText = "名前";
            dataGridView1.Columns[(int)GridCols.Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[(int)GridCols.Proj].HeaderText = "物件";
            dataGridView1.Columns[(int)GridCols.Proj].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[(int)GridCols.Assigned].HeaderText = "担当";
            dataGridView1.Columns[(int)GridCols.Assigned].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[(int)GridCols.Tag].HeaderText = "タグ";
            dataGridView1.Columns[(int)GridCols.State].HeaderText = "状態";
            dataGridView1.Columns[(int)GridCols.State].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[(int)GridCols.From].HeaderText = "開始";
            dataGridView1.Columns[(int)GridCols.From].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[(int)GridCols.Days].HeaderText = "人日";
            dataGridView1.Columns[(int)GridCols.Days].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[(int)GridCols.To].HeaderText = "終了";
            dataGridView1.Columns[(int)GridCols.To].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[(int)GridCols.Description].HeaderText = "備考";
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Columns[(int)GridCols.Description].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns[(int)GridCols.Description].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void SetGridViewRows()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                WorkItem wi = _list.ElementAt(i);
                dataGridView1.Rows.Add(
                    wi.Name,
                    wi.Project,
                    wi.AssignedMember,
                    wi.Tags,
                    wi.State,
                    wi.Period.From,
                    wi.Period.To,
                    _viewData.Original.Callender.GetPeriodDayCount(wi.Period),
                    wi.Description
                    );
            }
        }

        private bool Equal(WorkItem wi, DataGridViewCellCollection rowCells)
        {
            if (wi.Name == rowCells[(int)GridCols.Name].Value.ToString() &&
                wi.Project.ToString() == rowCells[(int)GridCols.Proj].Value.ToString() &&
                wi.AssignedMember.ToString() == rowCells[(int)GridCols.Assigned].Value.ToString() &&
                wi.Tags.ToString() == rowCells[(int)GridCols.Tag].Value.ToString() &&
                wi.State.ToString() == rowCells[(int)GridCols.State].Value.ToString() &&
                wi.Period.From.ToString() == rowCells[(int)GridCols.From].Value.ToString() &&
                wi.Period.To.ToString() == rowCells[(int)GridCols.To].Value.ToString() &&
                _viewData.Original.Callender.GetPeriodDayCount(wi.Period).ToString() == rowCells[(int)GridCols.Days].Value.ToString()
                ) return true;

            return false;
        }

        private int GetListIndexSelected(DataGridViewSelectedRowCollection selectedRows)
        {
            if (selectedRows.Count <= 0) return -1;
            for (int result = 0; result < _list.Count; result++)
            {
                if (Equal(_list[result], selectedRows[0].Cells)) return result;
            }
            return -1;
        }

        private void EditDataGridView(int index, WorkItem wi)
        {
            if (index < 0) return;
            _list[index] = wi;
            UpdateDataGridView();
            dataGridView1.Rows[index].Selected = true;
        }
    }
}
