﻿using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectsTM.Service
{
    class GitCmd
    {
        internal static void GitFetch(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath)) return;
            GitCmdCommon.GitCommand("-C " + gitRepoPath + " fetch");
        }
 
        private static string GetCurrentBranchName()
        {
            StringReader reader = new StringReader(GitCmdCommon.GitCommand("rev-parse --abbrev-ref Head"));
            return reader.ReadLine();
        }

        internal static int GetLocalBranchDate(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return -1;
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath)) return -1;
            return ParseDate(GitCmdCommon.GitCommand("-C " + gitRepoPath + " log -1 --date=short --pretty=format:%cd -- " + filePath));
        }

        internal static int GetRemoteBranchDate(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return -1;
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath)) return -1;
            return ParseDate(GitCmdCommon.GitCommand("-C " + gitRepoPath + " log -1 remotes/origin/" + GetCurrentBranchName() + " --date=short --pretty=format:%cd -- " + filePath));
        }

        internal static int GetLocalBranchCommitTime(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return -1;
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath)) return -1;
            return ParseTime(GitCmdCommon.GitCommand("-C " + gitRepoPath + " log -1 --pretty=format:%cd -- " + filePath));
        }

        internal static int GetRemoteBranchCommitTime(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return -1;
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath)) return -1;
            return ParseTime(GitCmdCommon.GitCommand("-C " + gitRepoPath + " log -1 remotes/origin/" + GetCurrentBranchName() + " --pretty=format:%cd -- " + filePath));
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