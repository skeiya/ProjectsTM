using System;
using System.Diagnostics;

namespace ProjectsTM.Service
{
    class GitCmdCommon
    {
        internal static int executeCommand(out string output, string command, string arguments = "")
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

        internal static string gitCommand(string arguments)
        {
            string output;
            if (executeCommand(out output, "git", arguments) != 0)
            {
                throw new Exception("git command error");
            }
            return output;
        }
    }
}
