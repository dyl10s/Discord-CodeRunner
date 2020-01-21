using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CodeRunner
{
    public class LanguageRunner
    {
        private Process[] startupProcesses;

        public string Run(string code)
        {
            if (code.Contains("```"))
            {
                code = code.Split("```")[1];
                StartupRun();
                var results = RunCode(code);
                EndRun();
                return results;
            }

            return null;
        }

        private void StartupRun()
        {
            Console.WriteLine("Running Code");
            startupProcesses = Process.GetProcesses();
        }

        private void EndRun()
        {
            foreach (var p in Process.GetProcesses())
            {
                var alreadyWasRunning = false;

                foreach (Process sameProc in startupProcesses)
                {
                    if (p.Id == sameProc.Id)
                    {
                        alreadyWasRunning = true;
                    }
                }

                if (!alreadyWasRunning)
                {
                    try
                    {
                        p.Kill(true);
                    }
                    catch { }
                }
            }
        }


        protected virtual string RunCode(string code)
        {
            return "Not Implimented";
        }
    }
}
