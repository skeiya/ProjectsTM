using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Main
{
    public partial class SuggestUserNameSettting : Form
    {
        public DialogResult Result { get; private set; } = DialogResult.No;
        public bool HideSetting { get; private set; } = false;

        public SuggestUserNameSettting()
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
