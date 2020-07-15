using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using ProjectsTM.Model;
using ProjectsTM.ViewModel;

namespace ProjectsTM.UI
{
    public partial class EditMileStoneForm : Form
    {
        private readonly Callender _callender;
        private readonly ViewData _viewData;
        private MileStone _mileStone;
        private IEnumerable<Project> _projects;

        public MileStone MileStone => _mileStone;

        public EditMileStoneForm(Callender callender, MileStone m, ViewData viewData)
        {
            InitializeComponent();
            this._callender = callender;
            this._viewData = viewData;
            ComboBox1_Init(m);
            if (m == null) return;
            textBoxName.Text = m.Name;
            textBoxDate.Text = m.Day.ToString();
            labelColor.BackColor = m.Color;
        }

        private void SetComboBoxItems()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("ALL");
            _projects = _viewData.Original.WorkItems.Select(w => w.Project).Distinct();
            foreach (var pro in _projects) comboBox1.Items.Add(pro.ToString());
        }

        private bool ComboBox1_Contain(MileStone m)
        {
            if (m == null ||
                m.Project == null ||
                m.Project.ToString() == null ||
                !comboBox1.Items.Contains(m.Project.ToString())) return false;
            return true;
        }

        private void ComboBox1_Init(MileStone m)
        {
            SetComboBoxItems();
            if (!ComboBox1_Contain(m)) { comboBox1.SelectedIndex = 0; return; }
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf(m.Project.ToString());  
        }

        private void ButtonSelectColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                labelColor.BackColor = dlg.Color;
            }
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

        private Project GetProject()
        {
            var selectedIndex = comboBox1.SelectedIndex;
            if (selectedIndex == 0) return new Project("ALL");
            return _projects.ElementAt(selectedIndex - 1);
        }

        private MileStone CreateMileStone()
        {
            var day = CallenderDay.Parse(textBoxDate.Text);
            if (!_callender.Days.Contains(day)) return ErrorMsg_NonWokingDay();
            return new MileStone(textBoxName.Text, day, labelColor.BackColor, GetProject());
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
