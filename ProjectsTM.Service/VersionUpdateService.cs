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
            if (!TryGetAppLatestVersion(dir, out var latestVersion)) return false;
            if (!IsOldCurrentVersion(latestVersion)) return false;
            if (MessageBox.Show("ツールの最新版がリリースされています。配布先を開きますか？", "日程表ツール", MessageBoxButtons.YesNo) != DialogResult.Yes) return false;
            Process.Start(GetFileServerPath(dir));
            Environment.Exit(0);
            return true;
        }

        private static bool TryGetAppLatestVersion(string dir, out Version result)
        {
            result = new Version();
            var fileServerPath = GetFileServerPath(dir);
            if (string.IsNullOrEmpty(fileServerPath)) return false;
            if (!Directory.Exists(fileServerPath)) return false;
            foreach (var d in Directory.GetDirectories(fileServerPath))
            {
                if (!TryParseVersion(d, out var version)) continue;
                if (result < version) result = version;
            }
            return true;
        }

        private static string GetFileServerPath(string dir)
        {
            var definedText = Path.Combine(dir, "UpdaterPlace.txt");
            if (!File.Exists(definedText)) return string.Empty;
            var lines = File.ReadAllLines(definedText);
            if (lines.Length < 1) return string.Empty;
            return lines[0];
        }

        private static bool IsOldCurrentVersion(Version latestVer)
        {
            if (latestVer == null) return false;
            var currentVer = Assembly.GetEntryAssembly().GetName().Version;
            return currentVer < latestVer;
        }

        private static bool TryParseVersion(string latestVersion, out Version result)
        {
            result = new Version();
            if (string.IsNullOrEmpty(latestVersion)) return false;
            var m = Regex.Match(latestVersion, @"v(\d)\.(\d)\.(\d)");
            if (!m.Success) return false;
            result = new Version(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
            return true;
        }
    }
}
