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

        public EditMileStoneForm(Callender _callender)
        {
            InitializeComponent();
            this._callender = _callender;
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

        private MileStone CreateMileStone()
        {
            var day = CallenderDay.Parse(textBoxDate.Text);
            if (!_callender.Days.Contains(day)) return null;
            return new MileStone(textBoxName.Text, day, labelColor.BackColor);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
