using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OpsJobBot.Ops
{
    public class SystemOps
    {
        static public void ExecuteCommand(string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);

            processInfo.CreateNoWindow  = true;
            processInfo.UseShellExecute = false;

            processInfo.RedirectStandardError = processInfo.RedirectStandardOutput = true;

            try
            {
                using (Process process = Process.Start(processInfo))
                {
                    process.WaitForExit();

                    int exitCode = process.ExitCode;

                    if (exitCode != 0)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error  = process.StandardError.ReadToEnd();

                        throw new Exception("ERROR!  Error Code(" + exitCode + ") : Error Msg(" + error + ")");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        static public bool IsJobRunning(string jobDirectory, string jobName)
        {
            bool isRunning = false;

            Process[] JobPossibilities = Process.GetProcessesByName(jobName);

            if ((JobPossibilities != null) && JobPossibilities.Length > 0)
            {
                isRunning = 
                    JobPossibilities.FirstOrDefault(p => p.MainModule.FileName.StartsWith(jobDirectory)) != default(Process);
            }

            /*
            bool isRunning = Process.GetProcessesByName(jobName)
                            .FirstOrDefault(p => p.MainModule.FileName.StartsWith(jobDirectory)) != default(Process);
            */

            return isRunning;
        }
    }
}