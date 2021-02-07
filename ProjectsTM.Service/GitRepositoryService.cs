﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectsTM.Service
{
    public static class GitRepositoryService
    {
        public static Task<bool> HasUnmergedRemoteCommit(string filePath)
        {
            Task<bool> task = Task.Run(() =>
            {
                if (!IsActive()) return false;
                if (string.IsNullOrEmpty(filePath)) return false;
                var repo = GitCmdRepository.FromFilePath(filePath);
                if (repo == null) return false;
                return IsThereUnmergedRemoteCommits(repo);
            }
            );
            return task;
        }

        public static Task<bool> HasUnpushedCommit(string filePath)
        {
            Task<bool> task = Task.Run(() =>
            {
                if (!IsActive()) return false;
                if (string.IsNullOrEmpty(filePath)) return false;
                var repo = GitCmdRepository.FromFilePath(filePath);
                if (repo == null) return false;
                return !IsUnpushedCommitEmpty(repo);
            }
            );
            return task;
        }

        public static Task<bool> HasUncommittedChange(string filePath)
        {
            Task<bool> task = Task.Run(() =>
            {
                if (!IsActive()) return false;
                if (string.IsNullOrEmpty(filePath)) return false;
                var repo = GitCmdRepository.FromFilePath(filePath);
                if (repo == null) return false;
                return !IsUncommitChangeEmpty(repo);
            }
            );
            return task;
        }

        public static bool IsActive()
        {
            return !string.IsNullOrEmpty(GitCmdRepository.GetVersion());
        }

        private static bool IsThereUnmergedRemoteCommits(GitCmdRepository repo)
        {
            repo.Fetch();
            var branchName = repo.GetCurrentBranchName();
            if (string.IsNullOrEmpty(branchName)) return false;
            var remoteName = repo.GetRemoteBranchName();
            if (string.IsNullOrEmpty(remoteName)) return false;
            var diff = repo.GetDifferenceBitweenBranches(branchName, remoteName);
            return 0 < ParseCommitsCount(diff);
        }

        private static int ParseCommitsCount(string str)
        {
            var matches = Regex.Matches(str, @"^commit ........................................");
            return matches.Count;
        }

        private static string ParseCommitId(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var matche = Regex.Match(str, @"^commit ........................................");
            if (!matche.Success) return string.Empty;
            return matche.Value.Replace("commit ", string.Empty);
        }

        public static bool TryAutoPull(string filePath)
        {
            var repo = GitCmdRepository.FromFilePath(filePath);
            if (repo == null) return false;
            if (!IsLocalChangeEmpty(repo)) return false;
            return repo.Pull();
        }

        /// <summary>
        /// ローカルの変更が無いことを確認できた場合のみtrueを変えす。異常時含め、それ以外のはすべてfalseを返す。
        /// </summary>
        private static bool IsLocalChangeEmpty(GitCmdRepository repo)
        {
            if (!IsUncommitChangeEmpty(repo)) return false;
            if (!IsUnpushedCommitEmpty(repo)) return false;
            return true;
        }

        private static bool IsUnpushedCommitEmpty(GitCmdRepository repo)
        {
            repo.Fetch();
            var branchName = repo.GetCurrentBranchName();
            if (string.IsNullOrEmpty(branchName)) return false;
            var remoteName = repo.GetRemoteBranchName();
            if (string.IsNullOrEmpty(remoteName)) return false;
            var diff = repo.GetDifferenceBitweenBranches(remoteName, branchName);
            return 0 == ParseCommitsCount(diff);
        }

        private static bool IsUncommitChangeEmpty(GitCmdRepository repo)
        {
            return string.IsNullOrEmpty(repo.Status());
        }

        public static string GetOldFileContentSomeMonthsAgo(string filePath, int months)
        {
            var commitId = ParseCommitId(GitCmdRepository.GitOldCommitMonthsAgo(filePath, months));
            return GitCmdRepository.GetOldFileContent(filePath, commitId);
        }

        public static string GetLastUpdateDateAndUserName(string filePath, int lineNo)
        {
            var lastUpdateDateAndUserName = ParseLastUpdateDateAndUserName(GitCmdRepository.GitBlameOneLine(filePath, lineNo));
            return lastUpdateDateAndUserName;
        }

        private static string ParseLastUpdateDateAndUserName(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var matche = Regex.Match(str, @"........ .*           <HashCode>.*</HashCode>");
            if (!matche.Success) return string.Empty;
            return Regex.Replace(str, ".............           <HashCode>.*</HashCode>", string.Empty);
        }
    }
}
