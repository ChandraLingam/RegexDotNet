using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace TestProjectShell
{
    [TestClass]
    public class HealthCareSolution
    {
        // TODO: Update path to point to your directory
        string _fileDirectory = @"C:\Git\RegexDotNet\Data\HealthData";

        public void ParseHealthDataTable(string fileName)
        {
            // TODO:  Implement parsing Logic
        }

        [TestMethod]
        public void ProcessFiles()
        {
            string[] fileList = {"problems.html", "labresults.html"};
            foreach (string file in fileList)
            {
                Console.WriteLine($"****{file}");
                ParseHealthDataTable(_fileDirectory + @"\" + file);
            }
        }
    }
}
