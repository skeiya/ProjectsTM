using ProjectsTM.Model;
using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Common
{
    public partial class EditMemberForm : Form
    {
        public EditMemberForm(string value, Member.MemberState state)
        {
            InitializeComponent();
            textBox1.Text = value;
            foreach(var s in System.Enum.GetValues(typeof(Member.MemberState)))
            {
                comboBox1.Items.Add(s.ToString());
            }
            comboBox1.SelectedIndex = (int)state;
        }

        public string EditText => textBox1.Text;
        public int Selected => comboBox1.SelectedIndex;

        public bool ReadOnly
        {
            set
            {
                textBox1.ReadOnly = value;
            }
            get
            {
                return textBox1.ReadOnly;
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
