using System.Linq;
using System.Windows.Forms;
using TaskManagement.Model;

namespace TaskManagement.UI
{
    public partial class SearchWorkitemForm : Form
    {
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
            Count,
        }

        private void SetGridViewColumns()
        {
            dataGridView1.ColumnCount = (int)GridCols.Count;
            dataGridView1.Columns[(int)GridCols.Name].HeaderText = "名前";
            dataGridView1.Columns[(int)GridCols.Proj].HeaderText = "物件";
            dataGridView1.Columns[(int)GridCols.Assigned].HeaderText = "担当";
            dataGridView1.Columns[(int)GridCols.Tag].HeaderText = "タグ";
            dataGridView1.Columns[(int)GridCols.State].HeaderText = "状態";
            dataGridView1.Columns[(int)GridCols.From].HeaderText = "開始";
            dataGridView1.Columns[(int)GridCols.Days].HeaderText = "人日";
            dataGridView1.Columns[(int)GridCols.To].HeaderText = "終了";
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
                    _viewData.Original.Callender.GetPeriodDayCount(wi.Period));
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
        }
    }
}
