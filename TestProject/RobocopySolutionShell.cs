using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

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
        string _fileDirectory = @"C:\Git\RegexDotNet\Data\RobocopyLog";

        [TestMethod]
        public void ProcessLogFiles()
        {
            string[] fileList = {"rocopylog_invalid_source.txt", "rocopylog.txt" };

            // TODO: Implement parsing Logic

            return;
        }
    }
}
