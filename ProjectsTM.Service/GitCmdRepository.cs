using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace ProjectsTM.Service
{
    class GitCmdRepository
    {
        private readonly string _repositoryDir;
        private GitCmdRepository(string repositoryDir)
        {
            _repositoryDir = repositoryDir;
        }
        public static GitCmdRepository FromFilePath(string path)
        {
            var repositoryDir = SearchGitRepoDir(path);
            if (repositoryDir == null) return null;
            return new GitCmdRepository(repositoryDir);
        }

        public static string GitBlameOneLine(string path, int lineNo)
        {
            var dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir)) return string.Empty;
            var reader = new StringReader(GitCommandRaw("-C " + dir + " blame -L " + lineNo + ",+1 " + path));
            return reader.ReadLine();
        }

        public static string GitOldCommitMonthsAgo(string path, int months)
        {
            var dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir)) return string.Empty;
            var reader = new StringReader(GitCommandRaw("-C " + dir + " log -1 --before=" + months.ToString() + ".month " + path));
            return reader.ReadLine();
        }

        public static string GetOldFileContent(string path, string commitId)
        {
            var dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir) | string.IsNullOrEmpty(commitId)) return string.Empty;
            var reader = new StringReader(GitCommandRaw("-C " + dir + " show " + commitId + ":./" + Path.GetFileName(path)));
            return reader.ReadToEnd();
        }

        private static string SearchGitRepoDir(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (dir == null) return string.Empty;
            var reader = new StringReader(GitCommandRaw("-C " + dir + " rev-parse --show-toplevel"));
            return reader.ReadLine();
        }

        public static string GetVersion()
        {
            var reader = new StringReader(GitCommandRaw("--version"));
            return reader.ReadLine();
        }

        internal string GetCurrentBranchName()
        {
            var reader = new StringReader(GitCommandRepository(" rev-parse --abbrev-ref Head"));
            return reader.ReadLine();
        }

        internal void Fetch()
        {
            if (string.IsNullOrEmpty(_repositoryDir)) return;
            GitCommandRepository(" fetch");
        }

        internal string GetDifferenceBitweenBranches(string baseBranch, string targetBranch)
        {
            return GitCommandRepository(" log " + baseBranch + ".." + targetBranch);
        }

        internal string GetRemoteBranchName()
        {
            var reader = new StringReader(GitCommandRepository(" rev-parse --abbrev-ref --symbolic-full-name @{u}"));
            return reader.ReadLine();
        }

        internal bool Pull()
        {
            return string.IsNullOrEmpty(GitCommandRepository(" pull"));
        }

        private static int ExecuteCommand(out string output, string command, string arguments)
        {
            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo(command)
                    {
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };
                    process.Start();

                    var tmp = new StringBuilder();
                    while (!process.HasExited)
                    {
                        tmp.Append(process.StandardOutput.ReadToEnd());
                        Thread.Sleep(0);
                    }
                    process.WaitForExit();
                    tmp.Append(process.StandardOutput.ReadToEnd());
                    output = tmp.ToString();
                    return process.ExitCode;
                }
            }
            catch
            {
                output = string.Empty;
                return -1;
            }
        }

        private string GitCommandRepository(string arguments)
        {
            return GitCommandRaw(" -C " + _repositoryDir + " " + arguments);
        }

        private static string GitCommandRaw(string arguments)
        {
            if (ExecuteCommand(out string output, "git", " --no-pager " + arguments) != 0)
            {
                return string.Empty;
            }
            return output;
        }

        internal string Status()
        {
            var reader = new StringReader(GitCommandRepository(" status -s"));
            return reader.ReadLine();
        }
    }
}
