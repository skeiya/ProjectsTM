using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System.Linq;
using System.Windows.Forms;

namespace ProjectsTM.UI
{
    public partial class TaskListForm : Form
    {
        private readonly ViewData _viewData;
        private PatternHistory _history;

        public TaskListForm(ViewData viewData, PatternHistory patternHistory)
        {
            InitializeComponent();

            this._viewData = viewData;
            this._history = patternHistory;
            gridControl1.Initialize(viewData, comboBoxPattern.Text);
            var offset = gridControl1.GridWidth - gridControl1.Width;
            this.Width += (int)offset;
        }

        internal void Clear()
        {
            comboBoxPattern.Text = string.Empty;
            gridControl1.Initialize(_viewData, null);
        }

        private void comboBoxPattern_DropDown(object sender, System.EventArgs e)
        {
            comboBoxPattern.Items.Clear();
            comboBoxPattern.Items.AddRange(_history.Items.ToArray());
        }

        private void buttonUpdate_Click(object sender, System.EventArgs e)
        {
            _history.Append(comboBoxPattern.Text);
            gridControl1.Initialize(_viewData, comboBoxPattern.Text);
        }
    }
}
