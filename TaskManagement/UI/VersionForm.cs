using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace TaskManagement.UI
{
    public partial class VersionForm : Form
    {
        public VersionForm()
        {
            InitializeComponent();
            labelVersion.Text = GetVersion();
        }

        private string GetVersion()
        {
            var ver = Assembly.GetEntryAssembly().GetName().Version;
            return "v" + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/TaskManagement/releases");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/TaskManagement");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/TaskManagement/graphs/contributors");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/TaskManagement/issues");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/TaskManagement/blob/master/LICENSE");
        }
    }
}
