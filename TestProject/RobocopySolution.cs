using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TestProject
{
    public class LogMetrics
    {
        public string logFileName = string.Empty;
        public Dictionary<string,string> directory = new Dictionary<string, string> ();
        public List<Dictionary<string,string>> metrics = new List<Dictionary<string, string>>();
        public bool error = false;
        public string errorMessage = string.Empty;
    }

    [TestClass]
    public class RobocopySolution
    {
        // TODO: Update path to point to your directory
        string directory = @"C:\Git\RegexDotNet\Data\RobocopyLog";
        string[] fileList = { "rocopylog_invalid_source.txt", "rocopylog.txt" };

        // Capture source and destination directory
        const string PATTERN_DIRECTORY_NAME = @"(?i)^\s+(?<type>Source|Dest)\s+:\s+(?<dir>.+)";
    
        // Capture Errors
        const string PATTERN_ERROR = @"(?i)^(?<ts>\d{4}(/\d{2}){2}\s+(\d{2}:){2}\d{2})\s+error(?<error>.+)";

        // Capture Metrics Columns
        const string PATTERN_METRICS =
            @"(?i)^\s+(?<type>dirs|files|bytes)\s+:\s+" +
            @"(?<total>\d{1,})\s+(?<copied>\d{1,})\s+" +
            @"(?<skipped>\d{1,})\s+(?<mismatch>\d{1,})\s+" +
            @"(?<failed>\d{1,})\s+(?<extras>\d{1,})";

        // Using compiled object
        //   Dynamically read group names
        Regex regexMetrics = new Regex(PATTERN_METRICS);

        private void ProcessRobocopyLog(string file)
        {
            LogMetrics logMetrics = new LogMetrics();
            logMetrics.logFileName = file;

            // Add table headers 
            List<string> metricGroupNames = new List<string>(regexMetrics.GetGroupNames());
            // Remove Group 0
            metricGroupNames.RemoveAt(0);

            using (StreamReader rdr = new StreamReader(file))
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
                    match = regexMetrics.Match(currentLine);

                    if (match.Success)
                    {
                        Dictionary<string,string> metricsRow = new Dictionary<string, string> ();

                        foreach (string name in metricGroupNames)
                        {
                            // Console.WriteLine($"{name}, {match.Groups[name].Value}");
                            metricsRow[name] = match.Groups[name].Value;
                        }

                        logMetrics.metrics.Add(metricsRow);
                    }
                }
            }

            // Write to a JSON file
            File.WriteAllText(file + ".json",
                JsonConvert.SerializeObject(logMetrics, Formatting.Indented));
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
