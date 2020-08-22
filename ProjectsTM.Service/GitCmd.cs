using System;
using System.Diagnostics;
using System.IO;

namespace ProjectsTM.Service
{
    class GitCmd
    {
        public static bool IsActive
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(GitCommand("--version"));
                }
                catch
                {
                    return false;
                }
            }
        }

        internal static void Fetch(string gitRepoPath)
        {
            if (string.IsNullOrEmpty(gitRepoPath)) return;
            GitCommand("-C " + gitRepoPath + " fetch");
        }

        internal static string GetCurrentBranchName(string gitRepoPath)
        {
            var reader = new StringReader(GitCommand("-C " + gitRepoPath + " rev-parse --abbrev-ref Head"));
            return reader.ReadLine();
        }

        internal static string GetDifferenceBitweenBranches(string gitRepoPath, string baseBranch, string targetBranch)
        {
            return GitCommand("-C " + gitRepoPath + " log " + baseBranch + ".." + targetBranch);
        }

        internal static string GetRemoteBranchName(string gitRepoPath)
        {
            var reader = new StringReader(GitCommand("-C " + gitRepoPath + " rev-parse --abbrev-ref --symbolic-full-name @{u}"));
            return reader.ReadLine();
        }

        private static int ExecuteCommand(out string output, string command, string arguments = "")
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(command)
                {
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.WaitForExit();
            output = process.StandardOutput.ReadToEnd();
            return process.ExitCode;
        }

        internal static string GitCommand(string arguments)
        {
            string output;
            if (ExecuteCommand(out output, "git", arguments) != 0)
            {
                return string.Empty;
            }
            return output;
        }

        internal static string GetRepositoryPath(string dir)
        {
            var reader = new StringReader(GitCommand("-C " + dir + " rev-parse --show-toplevel"));
            return reader.ReadLine();
        }
    }
}
