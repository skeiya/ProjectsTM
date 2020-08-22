using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectsTM.Service
{
    class GitCmd
    {
        internal static void Fetch(string filePath)
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

        internal static int GetDifferentCommitsCount(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return 0;
            var gitRepoPath = SearchGitRepo(filePath);
            var branchName = GetCurrentBranchName();
            return ParseCommitsCount(GitCmdCommon.GitCommand("-C " + gitRepoPath + " log " + branchName + "..remotes/origin/" + branchName + " -- " + filePath));
        }

        private static int ParseCommitsCount(string str)
        {
            var matches = Regex.Matches(str, @"commit ........................................");
            return matches.Count;
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
