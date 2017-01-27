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
    }
}