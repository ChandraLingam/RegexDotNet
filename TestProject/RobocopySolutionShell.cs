using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Diagnostics;

// Different namespace for hands-on-labs to prevent class naming conflict
namespace TestProjectShell
{

    /// <summary>
    /// Implement Log Parsing Code
    /// </summary>
   
    // TODO: Define LogMetrics class


    [TestClass]
    public class RobocopySolution
    {
        // TODO: Update path to point to your directory
        string directory = @"C:\Git\RegexDotNet\Data\RobocopyLog";
        string[] fileList = { "rocopylog_invalid_source.txt", "rocopylog.txt" };

        private void ProcessRobocopyLog(string file)
        {
            // TODO: Implement parsing Logic
            return;
        }

        [TestMethod]
        public void ProcessLogFiles()
        {
            Stopwatch stopWatch = new Stopwatch();

            foreach (string file in fileList)
            {
                Console.WriteLine($"File: {file}");
                Console.WriteLine();

                stopWatch.Start();
                ProcessRobocopyLog(directory + @"\" + file);
                stopWatch.Stop();

                Console.WriteLine($"  Elapsed Time: {stopWatch.ElapsedMilliseconds} ms");
                Console.WriteLine();

                stopWatch.Reset();
            }            
        }
    }
}
