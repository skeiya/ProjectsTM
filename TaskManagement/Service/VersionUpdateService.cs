using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TaskManagement.Service
{
    static class VersionUpdateService
    {
        public static bool Update()
        {
            var latestVersion = GetLatestVersion();
            if (!IsOldCurrentVersion(latestVersion)) return false;
            if (MessageBox.Show("ツールの最新版がリリースされています。配布先を開きますか？", "日程表ツール", MessageBoxButtons.YesNo) != DialogResult.Yes) return false;
            Process.Start(@"https://github.com/skeiya/TaskManagement/releases");
            return true;
        }

        private static bool IsOldCurrentVersion(string latestVersion)
        {
            var latestVer = ParseVersion(latestVersion);
            if (latestVer == null) return false;
            var currentVer = Assembly.GetEntryAssembly().GetName().Version;
            return currentVer < latestVer;
        }

        private static Version ParseVersion(string latestVersion)
        {
            if (latestVersion == null) return null;
            var m = Regex.Match(latestVersion, @"v(\d)\.(\d)\.(\d)");
            if (!m.Success) return null;
            return new Version(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
        }

        private static string GetLatestVersion()
        {
            try
            {
                var client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("TaskManagement"));
                var release = client.Repository.Release.GetLatest("skeiya", "TaskManagement").Result;
                return release.TagName;
            }
            catch
            {
                return null;
            }
        }
    }
}
