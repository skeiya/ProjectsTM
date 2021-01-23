using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    public partial class PromptUserNameSettting : Form
    {
        public DialogResult Result { get; set; } = DialogResult.No;
        public bool HideSetting { get; set; } = false;

        public PromptUserNameSettting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Yes;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Result = DialogResult.No;
            HideSetting = checkBox1.Checked;
            Close();
        }
    }
}
