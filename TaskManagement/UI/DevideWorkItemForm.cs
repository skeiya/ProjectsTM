using System;
using System.Windows.Forms;

namespace TaskManagement.UI
{
    public partial class DevideWorkItemForm : Form
    {
        private readonly int _originalCount;

        public DevideWorkItemForm(int count)
        {
            InitializeComponent();
            labelBefore.Text = count.ToString();
            this._originalCount = count;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValid()) return;
            }
            catch
            {
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool IsValid()
        {
            if (Devided <= 0) return false;
            if (Remain <= 0) return false;
            return true;
        }

        public int Devided => int.Parse(textBoxDevided.Text);
        public int Remain => _originalCount - Devided;

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TextBoxDevided_TextChanged(object sender, EventArgs e)
        {
            int devidedCount = 0;
            if (!int.TryParse(textBoxDevided.Text, out devidedCount)) return;
            labelRemain.Text = (_originalCount - devidedCount).ToString();
        }

    }
}
