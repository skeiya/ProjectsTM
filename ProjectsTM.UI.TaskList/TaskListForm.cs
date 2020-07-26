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
            gridControl1.ListUpdated += GridControl1_ListUpdated;
            gridControl1.Initialize(viewData, comboBoxPattern.Text, IsAudit());
            var offset = gridControl1.GridWidth - gridControl1.Width;
            this.Width += offset + gridControl1.VScrollBarWidth;
            this.Height = formSize?.TaskListFormHeight > this.Height ? formSize.TaskListFormHeight : this.Height;
            this._formSize = formSize;
            this.FormClosed += TaskListForm_FormClosed;
        }

        private void GridControl1_ListUpdated(object sender, System.EventArgs e)
        {
            UpdateLabelSum();
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
        }

        private bool IsAudit()
        {
            return radioButtonAudit.Checked;
        }

        public void Clear()
        {
            comboBoxPattern.Text = string.Empty;
            gridControl1.Initialize(_viewData, null, IsAudit());
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
            gridControl1.Initialize(_viewData, comboBoxPattern.Text, IsAudit());
        }

        private void radioButtonFilter_CheckedChanged(object sender, System.EventArgs e)
        {
            CheckUpdated();
        }

        private void radioButtonAudit_CheckedChanged(object sender, System.EventArgs e)
        {
            CheckUpdated();
        }

        private void CheckUpdated()
        {
            if (radioButtonAudit.Checked)
            {
                Audit();
            }
            else
            {
                UpdateList();
            }
        }

        private void Audit()
        {
            comboBoxPattern.Text = string.Empty;
            gridControl1.Initialize(_viewData, null, IsAudit());
        }
    }
}
