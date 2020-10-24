using ProjectsTM.UI.Common;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace ProjectsTM.UI.MainForm
{
    public partial class VersionForm : BaseForm
    {
        public VersionForm()
        {
            InitializeComponent();
            labelVersion.Text = GetVersion();
        }

        private static string GetVersion()
        {
            var ver = Assembly.GetEntryAssembly().GetName().Version;
            return "v" + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/ProjectsTM/releases");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/ProjectsTM");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/ProjectsTM/graphs/contributors");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/ProjectsTM/issues");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"https://github.com/skeiya/ProjectsTM/blob/master/LICENSE");
        }
    }
}
