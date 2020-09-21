using ProjectsTM.Model;
using ProjectsTM.ViewModel;
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
            var offset = gridControl1.GridWidth - gridControl1.Width;
            this.Width += offset + gridControl1.VScrollBarWidth;
            this.Height = formSize?.TaskListFormHeight > this.Height ? formSize.TaskListFormHeight : this.Height;
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
            var monthCount = dayCount / 20;
            labelSum.Text = dayCount.ToString() + "day (" + monthCount.ToString() + "人月)";
        }

        private void TaskListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _formSize.TaskListFormHeight = this.Height;
            _formSize.TaskListColWidths.Clear();
            for (var idx = 0; idx < gridControl1.ColCount; idx++)
            {
                _formSize.TaskListColWidths.Add(gridControl1.ColWidths[idx]);
            }
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
