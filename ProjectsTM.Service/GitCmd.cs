using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectsTM.Service
{
    class GitCmd
    {
        internal static void GitFetch(string filePath)
        {
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath) || string.IsNullOrEmpty(filePath)) return;
            GitCmdCommon.gitCommand("-C " + gitRepoPath + " fetch");
        }
 
        private static string GetCurrentBranchName()
        {
            StringReader reader = new StringReader(GitCmdCommon.gitCommand("rev-parse --abbrev-ref Head"));
            return reader.ReadLine();
        }

        internal static int GetLocalBranchDate(string filePath)
        {
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath) || string.IsNullOrEmpty(filePath)) return -1;
            return ParseDate(GitCmdCommon.gitCommand("-C " + gitRepoPath + " log -1 --date=short --pretty=format:%cd -- " + filePath));
        }

        internal static int GetRemoteBranchDate(string filePath)
        {
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath) || string.IsNullOrEmpty(filePath)) return -1;
            return ParseDate(GitCmdCommon.gitCommand("-C " + gitRepoPath + " log -1 remotes/origin/" + GetCurrentBranchName() + " --date=short --pretty=format:%cd -- " + filePath));
        }

        internal static int GetLocalBranchCommitTime(string filePath)
        {
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath) || string.IsNullOrEmpty(filePath)) return -1;
            return ParseTime(GitCmdCommon.gitCommand("-C " + gitRepoPath + " log -1 --pretty=format:%cd -- " + filePath));
        }

        internal static int GetRemoteBranchCommitTime(string filePath)
        {
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath) || string.IsNullOrEmpty(filePath)) return -1;
            return ParseTime(GitCmdCommon.gitCommand("-C " + gitRepoPath + " log -1 remotes/origin/" + GetCurrentBranchName() + " --pretty=format:%cd -- " + filePath));
        }

        private static int ParseDate(string output)
        {
            var m = Regex.Match(output, @"(\d)(\d)(\d)(\d)-(\d)(\d)-(\d)(\d)");
            if (!m.Success) return -1;
            return int.Parse(m.Value.Replace("-", ""));
        }

        private static int ParseTime(string output)
        {
            var m = Regex.Match(output, @"(\d)(\d):(\d)(\d):(\d)(\d)");
            if (!m.Success) return -1;
            return int.Parse(m.Value.Replace(":", ""));
        }

        internal static string SearchGitRepo(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (dir == null) return string.Empty;
            var repositoryPath = Directory.GetDirectories(dir, ".git");
            return repositoryPath.Count() == 0 ? SearchGitRepo(dir) : Path.GetDirectoryName(repositoryPath[0]);
        }
    }
}
