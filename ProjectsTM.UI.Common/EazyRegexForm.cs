using System;
using System.Windows.Forms;

namespace ProjectsTM.UI.Common
{
    public partial class EazyRegexForm : BaseForm
    {
        public EazyRegexForm()
        {
            InitializeComponent();
        }

        public string TaskName => textBoxTaskName.Text;
        public string ProjectName => textBoxProjectName.Text;
        public string MemberName => textBoxMemberName.Text;
        public string TagText => textBoxTagText.Text;

        public string RegexPattern
        {
            get
            {
                var result = @"^\[";
                result += string.IsNullOrEmpty(TaskName) ? ".*?" : ".*?" + TaskName + ".*?";
                result += @"\]\[";
                result += string.IsNullOrEmpty(ProjectName) ? ".*?" : ".*?" + ProjectName + ".*?";
                result += @"\]\[";
                result += string.IsNullOrEmpty(MemberName) ? ".*?" : ".*?" + MemberName + ".*?";
                result += @"\]\[";
                result += string.IsNullOrEmpty(TagText) ? ".*?" : ".*?" + TagText + ".*?";
                result += @"\]";
                return result;
            }
        }
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
