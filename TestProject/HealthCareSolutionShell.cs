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
        string directory = @"C:\Git\RegexDotNet\Data\HealthData";
        string[] fileList = { "problems.html", "labresults.html" };

        public void ProcessHealthCareData(string fileName)
        {
            // TODO:  Implement parsing Logic
        }

        [TestMethod]
        public void ProcessFiles()
        {
            foreach (string file in fileList)
            {
                Console.WriteLine($"****{file}");
                ProcessHealthCareData(directory + @"\" + file);
            }
        }
    }
}
