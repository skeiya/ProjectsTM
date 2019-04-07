using System;
using System.Windows.Forms;

namespace TaskManagement.UI
{
    public partial class EditMemberForm : Form
    {
        public EditMemberForm(string value)
        {
            InitializeComponent();
            textBox1.Text = value;
        }

        public string EditText => textBox1.Text;

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
