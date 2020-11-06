using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TestProject
{
    public class LogMetrics
    {
        public string logFileName;
        public Dictionary<string,string> directory = new Dictionary<string, string> ();
        public List<List<string>> metrics = new List<List<string>>();
        public bool error;
        public string errorMessage;
    }

    [TestClass]
    public class RobocopySolution
    {
        // TODO: Update path to point to your directory
        string _fileDirectory = @"C:\Git\RegexDotNet\Data\RobocopyLog";

        [TestMethod]
        public void ProcessLogFiles()
        {
            string[] fileList = {"rocopylog_invalid_source.txt", "rocopylog.txt" };

            string PATTERN_DIRECTORY_NAME = @"(?i)^\s+(?<type>Source|Dest)\s+:\s+(?<dir>.+)";
            
            string PATTERN_ERROR = @"(?i)^(?<ts>\d{4}(/\d{2}){2}\s+(\d{2}:){2}\d{2})\s+error(?<error>.+)";
            
            string PATTERN_METRICS = 
                @"(?i)^\s+(?<type>dirs|files|bytes)\s+:\s+" +
                @"(?<total>\d{1,})\s+(?<copied>\d{1,})\s+" +
                @"(?<skipped>\d{1,})\s+(?<mismatch>\d{1,})\s+" + 
                @"(?<failed>\d{1,})\s+(?<extras>\d{1,})";

            // Using compiled object
            //   Dynamically read group names
            Regex regexMetrics = new Regex(PATTERN_METRICS);

            foreach (string file in fileList)
            {
                LogMetrics logMetrics = new LogMetrics();

                logMetrics.logFileName = file;

                // Add table headers 
                List<string> metricGroupNames = new List<string>(regexMetrics.GetGroupNames());

                // Remove Group 0
                metricGroupNames.RemoveAt(0);

                logMetrics.metrics.Add(metricGroupNames);

                using (StreamReader rdr = new StreamReader(_fileDirectory + @"\" + file))
                {
                    while (!rdr.EndOfStream)
                    {
                        string currentLine = rdr.ReadLine();

                        // Check for errors
                        if (Regex.IsMatch(currentLine, PATTERN_ERROR))
                        {
                            logMetrics.error = true;
                            logMetrics.errorMessage = currentLine;
                        }

                        // Capture directory path
                        Match match = Regex.Match(currentLine, PATTERN_DIRECTORY_NAME);

                        if (match.Success)
                        {
                            logMetrics.directory.Add(match.Groups["type"].Value, match.Groups["dir"].Value);
                        }

                        // use compiled object
                        match =  regexMetrics.Match(currentLine);

                        if (match.Success)
                        {
                            List<string> metricsRow = new List<string>();

                            foreach (string name in metricGroupNames)
                            {
                                metricsRow.Add(match.Groups[name].Value);
                            }

                            logMetrics.metrics.Add(metricsRow);
                        }
                    }
                }

                // Write to a JSON file
                File.WriteAllText(_fileDirectory + @"\" + file + ".json", 
                    JsonConvert.SerializeObject(logMetrics, Formatting.Indented));
            }
        }
    }
}
