using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public partial class DivideWorkItemForm : Form
    {
        private readonly int _originalCount;

        public DivideWorkItemForm(int count)
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
            if (Divided <= 0) return false;
            if (Remain <= 0) return false;
            return true;
        }

        public int Divided => int.Parse(textBoxDivided.Text);
        public int Remain => _originalCount - Divided;

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TextBoxDivided_TextChanged(object sender, EventArgs e)
        {
            int dividedCount = 0;
            if (!int.TryParse(textBoxDivided.Text, out dividedCount)) return;
            labelRemain.Text = (_originalCount - dividedCount).ToString();
        }

    }
}
