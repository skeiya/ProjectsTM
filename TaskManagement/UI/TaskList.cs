using System;
using System.Windows.Forms;
using TaskManagement.Model;
using TaskManagement.ViewModel;

namespace TaskManagement.UI
{
    public partial class TaskList : Form
    {
        private ViewData _viewData = null;

        public TaskList(ViewData viewData)
        {
            InitializeComponent();
            _viewData = viewData;
        }

        private void SetGridViewColumns()
        {
            dataGridView1.ColumnCount = 8;
            dataGridView1.Columns[0].HeaderText = "名前";
            dataGridView1.Columns[1].HeaderText = "物件";
            dataGridView1.Columns[2].HeaderText = "担当";
            dataGridView1.Columns[3].HeaderText = "タグ";
            dataGridView1.Columns[4].HeaderText = "状態";
            dataGridView1.Columns[5].HeaderText = "開始";
            dataGridView1.Columns[6].HeaderText = "人日";
            dataGridView1.Columns[7].HeaderText = "終了";
        }

        private void SetGridViewRows()
        {
            WorkItems workItems = _viewData.GetFilteredWorkItems();
            foreach (var wi in workItems)
            {
                dataGridView1.Rows.Add(
                    wi.Name,
                    wi.Project,
                    wi.AssignedMember,
                    wi.Tags,
                    wi.State,
                    wi.Period.From,
                    _viewData.Original.Callender.GetPeriodDayCount(wi.Period),
                    wi.Period.To);
            }
        }

        private void InitDataGridView()
        {
            SetGridViewColumns();
            SetGridViewRows();
        }

        private void TaskList_Load(object sender, EventArgs e)
        {
            InitDataGridView();
        }
    }
}
