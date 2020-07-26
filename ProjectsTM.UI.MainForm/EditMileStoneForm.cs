using ProjectsTM.Model;
using ProjectsTM.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectsTM.UI
{
    public partial class EditMileStoneForm : Form
    {
        private readonly Callender _callender;
        private readonly ViewData _viewData;
        private MileStone _mileStone;
        private MileStoneFilters _mileStoneFilters;

        public MileStone MileStone => _mileStone;

        public EditMileStoneForm(Callender callender, MileStone m, ViewData viewData, MileStoneFilters mileStoneFilters)
        {
            InitializeComponent();
            this._callender = callender;
            this._viewData = viewData;
            this._mileStoneFilters = mileStoneFilters;
            ComboBox1_Init(m);
            if (m == null) return;
            textBoxName.Text = m.Name;
            textBoxDate.Text = m.Day.ToString();
            labelColor.BackColor = m.Color;
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
                msFilter.Name == null ||
                msFilter.Name == String.Empty ||
                !comboBox1.Items.Contains(msFilter.Name)) return false;
            return true;
        }

        private void ComboBox1_Init(MileStone m)
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
            return new MileStone(textBoxName.Text, day, labelColor.BackColor, new MileStoneFilter(comboBox1.Text));
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
