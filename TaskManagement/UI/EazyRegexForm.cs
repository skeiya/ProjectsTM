using System;
using System.Windows.Forms;

namespace TaskManagement.UI
{
    public partial class EazyRegexForm : Form
    {
        public EazyRegexForm()
        {
            InitializeComponent();
        }

        public string TaskName => textBoxTaskName.Text;
        public string ProjectName => textBoxProjectName.Text;
        public string MemberName => textBoxMemberName.Text;
        public string TagText => textBoxTagText.Text;

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
