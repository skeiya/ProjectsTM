using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectsTM.Service
{
    public static class GitRepositoryService
    {
        public static Task<bool> CheckRemoteBranchAppDataFile(string filePath)
        {
            Task<bool> task = Task.Run(() =>
            {
                if (!IsActive()) return false;
                if (string.IsNullOrEmpty(filePath)) return false;
                var gitRepoPath = SearchGitRepo(filePath);
                if (string.IsNullOrEmpty(gitRepoPath)) return false;
                return IsThereUnmergedRemoteCommits(gitRepoPath);
            }
            );
            return task;
        }

        public static bool IsActive()
        {
            return !string.IsNullOrEmpty(GitCmd.GetVersion());
        }

        private static bool IsThereUnmergedRemoteCommits(string gitRepoPath)
        {
            GitCmd.Fetch(gitRepoPath);
            var branchName = GitCmd.GetCurrentBranchName(gitRepoPath);
            if (string.IsNullOrEmpty(branchName)) return false;
            var remoteName = GitCmd.GetRemoteBranchName(gitRepoPath);
            if (string.IsNullOrEmpty(remoteName)) return false;
            var diff = GitCmd.GetDifferenceBitweenBranches(gitRepoPath, branchName, remoteName);
            return 0 < ParseCommitsCount(diff);
        }

        private static int ParseCommitsCount(string str)
        {
            var matches = Regex.Matches(str, @"^commit ........................................");
            return matches.Count;
        }

        private static string SearchGitRepo(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (dir == null) return string.Empty;
            return GitCmd.GetRepositoryPath(dir);
        }

        public static bool TryAutoPull(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            var gitRepoPath = SearchGitRepo(filePath);
            if (string.IsNullOrEmpty(gitRepoPath)) return false;
            if (IsThereAnyLocalChange(gitRepoPath)) return false;
            return GitCmd.Pull(gitRepoPath);
        }

        private static bool IsThereAnyLocalChange(string gitRepoPath)
        {
            GitCmd.Fetch(gitRepoPath);
            var branchName = GitCmd.GetCurrentBranchName(gitRepoPath);
            if (string.IsNullOrEmpty(branchName)) return false;
            var remoteName = GitCmd.GetRemoteBranchName(gitRepoPath);
            if (string.IsNullOrEmpty(remoteName)) return false;
            var diff = GitCmd.GetDifferenceBitweenBranches(gitRepoPath, remoteName, branchName);
            if (0 < ParseCommitsCount(diff)) return true;
            return !string.IsNullOrEmpty(GitCmd.Status(gitRepoPath));
        }
    }
}
