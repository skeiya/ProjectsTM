using System;
using System.Windows.Forms;
using TaskManagement.Model;

namespace TaskManagement.UI
{
    public partial class EditMileStoneForm : Form
    {
        private readonly Callender _callender;
        private MileStone _mileStone;

        public MileStone MileStone => _mileStone;

        public EditMileStoneForm(Callender callender, MileStone m)
        {
            InitializeComponent();
            this._callender = callender;
            if (m == null) return;
            textBoxName.Text = m.Name;
            textBoxDate.Text = m.Day.ToString();
            labelColor.BackColor = m.Color;
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

        private MileStone ErrorMsg_NonWokingDay()
        {
            MessageBox.Show("非稼働日です。稼働日を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private MileStone CreateMileStone()
        {
            var day = CallenderDay.Parse(textBoxDate.Text);
            if (!_callender.Days.Contains(day)) return ErrorMsg_NonWokingDay();
            return new MileStone(textBoxName.Text, day, labelColor.BackColor);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
