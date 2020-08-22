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
                if (!IsAcrive()) return false;
                if (string.IsNullOrEmpty(filePath)) return false;
                var gitRepoPath = SearchGitRepo(filePath);
                if (string.IsNullOrEmpty(gitRepoPath)) return false;
                return IsThereUnmergedRemoteCommits(gitRepoPath);
            }
            );
            return task;
        }

        private static bool IsAcrive()
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
    }
}
