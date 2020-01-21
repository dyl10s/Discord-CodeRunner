using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CodeRunner.Languages
{
    class jsRunner : LanguageRunner
    {
        protected override string RunCode(string code)
        {
            try
            {
                if (code.Contains("require(") || code.Contains("eval("))
                {
                    return "Require / Eval command detected, no bad stuff filter engaged!";
                }

                ProcessStartInfo procInfo = new ProcessStartInfo();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    procInfo.FileName = Directory.GetCurrentDirectory() + "\\Compilers\\node.exe";
                }
                else
                {
                    procInfo.FileName = Directory.GetCurrentDirectory().Replace("\\", "/") + "/Compilers/node";
                }

                procInfo.Arguments = "-e \"" + code.Replace("\"", "\\\"").Replace("\n", "") + "\"";
                procInfo.RedirectStandardOutput = true;
                Process p = new Process();
                p.StartInfo = procInfo;

                p.Start();
                var outputStream = p.StandardOutput;
                p.WaitForExit(1000);
                p.Kill();

                var codeResults = outputStream.ReadToEnd();
                return codeResults.Substring(0, Math.Min(codeResults.Length, 500));
            }
            catch (Exception e)
            {
                return "Error Running Code / Code Has No Output";
            }
        }
    }
}
