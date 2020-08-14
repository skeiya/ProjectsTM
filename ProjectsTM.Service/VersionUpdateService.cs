using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectsTM.Service
{
    public static class VersionUpdateService
    {
        public static bool UpdateByFileServer(string dir)
        {
            var latestVersion = GetLatestVersionByFileServer(dir);
            if (latestVersion == null) return false;
            if (!IsOldCurrentVersion(latestVersion)) return false;
            if (MessageBox.Show("ツールの最新版がリリースされています。配布先を開きますか？", "日程表ツール", MessageBoxButtons.YesNo) != DialogResult.Yes) return false;
            Process.Start(GetFileServerPath(dir));
            return true;
        }

        private static Version GetLatestVersionByFileServer(string dir)
        {
            Version result = null;
            var fileServerPath = GetFileServerPath(dir);
            if (fileServerPath == null) return null;
            if (!Directory.Exists(fileServerPath)) return null;
            foreach (var d in Directory.GetDirectories(fileServerPath))
            {
                var version = ParseVersion(d);
                if (result == null || result < version) result = version;
            }
            return result;
        }

        private static string GetFileServerPath(string dir)
        {
            var definedText = Path.Combine(dir, "UpdaterPlace.txt");
            if (!File.Exists(definedText)) return null;
            var lines = File.ReadAllLines(definedText);
            if (lines.Length < 1) return null;
            return lines[0];
        }

        private static bool IsOldCurrentVersion(Version latestVer)
        {
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
    }
}
