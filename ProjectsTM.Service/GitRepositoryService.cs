using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectsTM.Service
{
    public static class GitRepositoryService
    {
        private static string AppConfigDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ProjectsTM");

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
            var matche = Regex.Match(str, @"^commit ........................................");
            if (!matche.Success) return string.Empty;
            return matche.Value.Replace("commit ", "");
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
        /// <param name="repo"></param>
        /// <returns></returns>
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

        public static string GetOldFileSomeMonthsAgo(string filePath, int months)
        {
            var commitId = ParseCommitId(GitCmdRepository.GitOldCommitMonthsAgo(filePath, months));
            var text = GitCmdRepository.ReadOldFile(filePath, commitId);
            if (text == string.Empty) return string.Empty;
            var oldFilePath = Path.Combine(AppConfigDir, Path.GetFileName(filePath));
            return SaveFile(text, oldFilePath);
        }

        private static string SaveFile(string text, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, text);
            }
            catch { return string.Empty; }
            return filePath;
        }


    }
}
