using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TestProject
{
    public class LogMetricsSummary
    {
        public string logFileName;
        public Dictionary<string,string> directory = new Dictionary<string, string> ();
        public List<string> metricType = new List<string>();
        public List<int> total = new List<int>();
        public List<int> copied = new List<int>();
        public List<int> skipped = new List<int>();
        public List<int> failed = new List<int>();
        public bool error;
        public string errorMessage;
    }

    [TestClass]
    public class RobocopyLogExample
    {               
        [TestMethod]
        public void ParseLogTest()
        {
            string[] fileList = {
                @"C:\RegexCourse\Data\RobocopyLog\rocopylog_invalid_source.txt",
               @"C:\RegexCourse\Data\RobocopyLog\rocopylog.txt" };

            string PATTERN_DIRECTORY_NAME = @"^\s+(?<type>Source|Dest)\s+:(?<dir>.+)";
            string PATTERN_ERROR = @"(?in)^(?<ts>\d{4}(/\d{2}){2}\s+(\d{2}:){2}\d{2})\s+error(?<error>.+)";            
            string PATTERN_METRICS = @"(?i)^\s+(?<type>dirs|files|bytes)\s+:\s+(?<total>\d{1,})\s+(?<copied>\d{1,})\s+(?<skipped>\d{1,})\s+(?<mismatch>\d{1,})\s+(?<failed>\d{1,})\s+(?<extras>\d{1,})";


            foreach (string file in fileList)
            {
                LogMetricsSummary logSummary = new LogMetricsSummary();
                logSummary.logFileName = file;

                using (StreamReader rdr = new StreamReader(file))
                {
                    while (!rdr.EndOfStream)
                    {
                        string currentLine = rdr.ReadLine();

                        if (Regex.IsMatch(currentLine, PATTERN_ERROR))
                        {
                            logSummary.error = true;
                            logSummary.errorMessage = currentLine;
                        }

                        Match m = Regex.Match(currentLine, PATTERN_DIRECTORY_NAME);

                        if (m.Success
                            && m.Groups["type"].Success)
                        {
                            logSummary.directory.Add(m.Groups["type"].Value, m.Groups["dir"].Value);
                        }

                        m = Regex.Match(currentLine, PATTERN_METRICS);

                        if (m.Success && m.Groups["type"].Success)
                        {
                            logSummary.metricType.Add(m.Groups["type"].Value);
                            logSummary.total.Add(int.Parse(m.Groups["total"].Value));
                            logSummary.copied.Add(int.Parse(m.Groups["copied"].Value));
                            logSummary.skipped.Add(int.Parse(m.Groups["skipped"].Value));
                            logSummary.failed.Add(int.Parse(m.Groups["failed"].Value));
                        }
                    }
                }

                // Print as JSON
                Console.WriteLine(JsonConvert.SerializeObject(logSummary,Formatting.Indented));
            }
        }
    }
}
