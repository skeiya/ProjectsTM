using ProjectsTM.Model;
using ProjectsTM.Service;
using ProjectsTM.UI.Common;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public partial class SearchWorkitemForm : Form
    {
        private readonly ViewData _viewData;
        private readonly WorkItemEditService _editService;
        private readonly PatternHistory _history;
        private List<Tuple<WorkItem, Color>> _list = new List<Tuple<WorkItem, Color>>();
        private FormSize _formSize;

        public SearchWorkitemForm(ViewData viewData, WorkItemEditService editService, PatternHistory history, FormSize formSize)
        {
            InitializeComponent();
            this._viewData = viewData;
            this._editService = editService;
            this._history = history;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            this.Height = formSize?.SearchWorkitemFormHeight > this.Height ? formSize.SearchWorkitemFormHeight : this.Height;
            this._formSize = formSize;
            this.FormClosed += SearchWorkitemForm_FormClosed;
        }

        private void SearchWorkitemForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _formSize.SearchWorkitemFormHeight = this.Height;
        }

        private void UpdatePatternHistory()
        {
            comboBoxPattern.Items.Clear();
            comboBoxPattern.Items.AddRange(_history.Items.ToArray());
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            if (dataGridView1.RowCount - 1 <= dataGridView1.SelectedRows[0].Index) return;
            var index = GetListIndexSelected(dataGridView1.SelectedRows);
            if (index < 0) return;
            var found = _viewData.GetFilteredWorkItems().FirstOrDefault(w => w.Equals(_list[index]));
            _viewData.Selected = found == null ? null : new WorkItems(found);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            if (e.ColumnIndex < 0 || dataGridView1.ColumnCount <= e.ColumnIndex || e.RowIndex < 0 || dataGridView1.RowCount - 1 <= e.RowIndex) return;
            var index = GetListIndexSelected(dataGridView1.SelectedRows);
            if (index < 0) return;
            var wi = _list[index].Item1;
            using (var dlg = new EditWorkItemForm(wi.Clone(), _viewData.Original.Callender, _viewData.GetFilteredMembers()))
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
            comboBoxPattern.Enabled = !checkBoxOverwrapPeriod.Checked;
            if (!checkBoxOverwrapPeriod.Checked)
            {
                checkBoxIncludeMilestone.Enabled = true;
                checkBoxCaseDistinct.Enabled = true;
                return;
            }
            checkBoxIncludeMilestone.Enabled = false;
            checkBoxCaseDistinct.Enabled = false;

            comboBoxPattern.Text = string.Empty;
            UpdateList(OverwrapedWorkItemsCollectService.Get(_viewData.Original.WorkItems));
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (IsSearchOverwrap())
            {
                CheckBoxOverwrapPeriod_CheckedChanged(null, null);
                return;
            }
            UpdateList(_viewData.GetFilteredWorkItems());
            _history.Append(comboBoxPattern.Text);
        }

        private void UpdateList(IEnumerable<WorkItem> workItems)
        {
            _list.Clear();
            try
            {
                foreach (var wi in workItems)
                {
                    if (!Regex.IsMatch(wi.ToString(), comboBoxPattern.Text, GetOption())) continue;
                    _list.Add(new Tuple<WorkItem, Color>(wi, GetColor(wi)));
                }
                AddMilestones();
            }
            catch { }
            // ↓TODO:matsukage 暫定対策
            ResortDataGridView resorter = new ResortDataGridView(dataGridView1);
            // ↑TODO:matsukage 暫定対策
            UpdateDataGridView();
            // ↓TODO:matsukage 暫定対策
            resorter.Resort();
            // ↑TODO:matsukage 暫定対策
        }

        private void AddMilestones()
        {
            if (checkBoxOverwrapPeriod.Checked) return;
            if (!checkBoxIncludeMilestone.Checked) return;
            foreach (var ms in _viewData.Original.MileStones)
            {
                var w = new WorkItem(
                    new Project("noProj"),
                    "↑" + ms.Name,
                    new Tags(new List<string>() { "" }),
                    new Period(ms.Day, ms.Day),
                    new Member(string.Empty, string.Empty, string.Empty, MemberState.Woking),
                    TaskState.Active,
                    string.Empty
                    );
                _list.Add(new Tuple<WorkItem, Color>(w, ms.Color));
            }
        }

        private static Color GetColor(WorkItem wi)
        {
            return wi.State == TaskState.Done ? Color.Gray : Color.White;
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

        // ↓TODO:matsukage 暫定対策
        private class ResortDataGridView
        {
            private DataGridView _dataGridView;
            private DataGridViewColumn _sortedColumn;
            private SortOrder _sortOrder;

            public ResortDataGridView(DataGridView dataGridView)
            {
                _dataGridView = dataGridView;
                _sortedColumn = dataGridView.SortedColumn;
                _sortOrder = dataGridView.SortOrder;
            }

            internal void Resort()
            {
                switch (_sortOrder)
                {
                    case SortOrder.Ascending:
                        _dataGridView.Sort(_sortedColumn, ListSortDirection.Ascending);
                        return;
                    case SortOrder.Descending:
                        _dataGridView.Sort(_sortedColumn, ListSortDirection.Descending);
                        return;
                    default:
                        return;
                }
            }
        }
        // ↑TODO:matsukage 暫定対策

        private void UpdateDataGridView()
        {
            // ↓TODO:matsukage 暫定対策
            ResortDataGridView resorter = new ResortDataGridView(dataGridView1);
            // ↑TODO:matsukage 暫定対策
            dataGridView1.Rows.Clear();
            SetGridViewRows();
            // ↓TODO:matsukage 暫定対策
            resorter.Resort();
            // ↑TODO:matsukage 暫定対策
            var dayCount = _list.Sum(item => _viewData.Original.Callender.GetPeriodDayCount(item.Item1.Period));
            var monthCount = dayCount / 20;
            labelSum.Text = dayCount.ToString() + "day (" + monthCount.ToString() + "人月)";
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            var newSelect = new WorkItems();
            foreach (var w in _viewData.GetFilteredWorkItems())
            {
                if (_list.Any(i => i.Item1.Equals(w))) newSelect.Add(w);
            }
            _viewData.Selected = newSelect;
        }

        private void buttonEasyRegex_Click(object sender, EventArgs e)
        {
            using (var dlg = new EazyRegexForm())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                comboBoxPattern.Text = dlg.RegexPattern;
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
                var wi = _list.ElementAt(i).Item1;
                var color = _list.ElementAt(i).Item2;
                var row = dataGridView1.Rows.Add(
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
                dataGridView1.Rows[row].DefaultCellStyle.BackColor = color;
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

        internal void Show(MainForm parent, bool checkOverWrap)
        {
            Show(parent);
            if (checkOverWrap)
            {
                checkBoxOverwrapPeriod.Checked = true;
            }
        }

        private int GetListIndexSelected(DataGridViewSelectedRowCollection selectedRows)
        {
            if (selectedRows.Count != 1) return -1;
            for (int result = 0; result < _list.Count; result++)
            {
                if (Equal(_list[result].Item1, selectedRows[0].Cells)) return result;
            }
            return -1;
        }

        private void EditDataGridView(int index, WorkItem wi)
        {
            if (index < 0) return;
            _list[index] = new Tuple<WorkItem, Color>(wi, GetColor(wi));
            UpdateDataGridView();
            dataGridView1.Rows[index].Selected = true;
        }

        private void checkBoxIncludeMilestone_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxOverwrapPeriod.Enabled = !checkBoxIncludeMilestone.Checked;
            if (!checkBoxIncludeMilestone.Checked) return;
            UpdateList(_viewData.GetFilteredWorkItems());
            dataGridView1.Sort(dataGridView1.Columns[(int)GridCols.To], System.ComponentModel.ListSortDirection.Ascending);
            _history.Append(comboBoxPattern.Text);
        }

        private void comboBoxPattern_DropDown(object sender, EventArgs e)
        {
            UpdatePatternHistory();
        }
    }
}
