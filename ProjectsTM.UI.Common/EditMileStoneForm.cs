using ProjectsTM.Model;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Common
{
    public partial class EditMileStoneForm : BaseForm
    {
        private readonly Callender _callender;
        private MileStone _mileStone;
        private readonly MileStoneFilters _mileStoneFilters;

        public MileStone MileStone => _mileStone;

        public EditMileStoneForm(Callender callender, MileStone m, MileStoneFilters mileStoneFilters)
        {
            InitializeComponent();
            this._callender = callender;
            this._mileStoneFilters = mileStoneFilters;
            InitializeFilterCombobox(m);
            InitializeStateCombobox(m);
            if (m == null) return;
            textBoxName.Text = m.Name;
            textBoxProject.Text = m.Project.ToString();
            textBoxDate.Text = m.Day.ToString();
            labelColor.BackColor = m.Color;
        }

        private void InitializeStateCombobox(MileStone m)
        {
            comboBoxState.Items.Clear();
            foreach (var e in Enum.GetValues(typeof(TaskState)))
            {
                comboBoxState.Items.Add(e);
            }
            comboBoxState.SelectedItem = m != null ? m.State : TaskState.Active;
        }

        private void SetComboBoxItems()
        {
            comboBox1.Items.Clear();
            if (_mileStoneFilters == null) return;
            foreach (var msFilter in _mileStoneFilters)
            {
                if (msFilter.Name == null) continue;
                comboBox1.Items.Add(msFilter.Name);
            }
        }

        private bool ComboBox1_Contain(MileStoneFilter msFilter)
        {
            if (msFilter == null ||
                string.IsNullOrEmpty(msFilter.Name) ||
                !comboBox1.Items.Contains(msFilter.Name)) return false;
            return true;
        }

        private void InitializeFilterCombobox(MileStone m)
        {
            SetComboBoxItems();
            if (!ComboBox1_Contain(m?.MileStoneFilter)) return;
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(m.MileStoneFilter.Name);
        }

        private void ButtonSelectColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                labelColor.BackColor = dlg.Color;
            }
        }
        private TaskState GetState()
        {
            return (TaskState)comboBoxState.SelectedItem;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            _mileStone = CreateMileStone();
            if (_mileStone == null) return;
            DialogResult = DialogResult.OK;
            Close();
        }

        private static MileStone ErrorMsg_NonWokingDay()
        {
            MessageBox.Show("非稼働日です。稼働日を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private MileStone CreateMileStone()
        {
            var day = CallenderDay.Parse(textBoxDate.Text);
            if (!_callender.Days.Contains(day)) return ErrorMsg_NonWokingDay();
            return new MileStone(textBoxName.Text, new Project(textBoxProject.Text), day, labelColor.BackColor, new MileStoneFilter(comboBox1.Text), GetState());
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
