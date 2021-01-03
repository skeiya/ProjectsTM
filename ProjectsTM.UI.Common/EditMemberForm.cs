using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Common
{
    public partial class EditMemberForm : BaseForm
    {
        public EditMemberForm(string value)
        {
            InitializeComponent();
            textBox1.Text = value;
        }

        public string EditText => textBox1.Text;

        public bool ReadOnly
        {
            get
            {
                return textBox1.ReadOnly;
            }
            set
            {
                textBox1.ReadOnly = value;
            }
        }

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
