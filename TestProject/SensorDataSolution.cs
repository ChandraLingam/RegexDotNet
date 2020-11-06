using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

namespace TestProject
{
    // Data for one day
    public class SensorData
    {
        public DateTime dateTime;

        public List<string> temperature = new List<string>();
        public List<string> humidity = new List<string>();
    }

    [TestClass]
    public class SensorDataSolution
    {
        // TODO: Update path to point to your directory
        string _fileDirectory = @"C:\Git\RegexDotNet\Data\SensorData";        

        static string PATTERN_HEADER = @"^(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
        static string PATTERN_DATA = @"(?<temperature>\d{4}|NNNN)(?<humidity>\d{3}|NNN)";

        Regex regexSensorValue = new Regex(PATTERN_DATA, RegexOptions.Compiled);

        public void ParseSensorData(string fileName)
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            using (StreamReader rdr = new StreamReader(fileName))
            {
                List<SensorData> allData = new List<SensorData>();

                while (!rdr.EndOfStream)
                {
                    SensorData sensorData = new SensorData();

                    string currentLine = rdr.ReadLine();

                    Match match = Regex.Match(currentLine, PATTERN_HEADER);

                    if (match.Success)
                    {
                        sensorData.dateTime = new DateTime(
                            int.Parse(match.Groups["year"].Value),
                            int.Parse(match.Groups["month"].Value),
                            int.Parse(match.Groups["day"].Value));

                        // Skip the header portion and look for temperature-humdity pairs
                        Match matchData = regexSensorValue.Match(currentLine, match.Value.Length);

                        while (matchData.Success)
                        {
                            sensorData.temperature.Add(matchData.Groups["temperature"].Value);
                            sensorData.humidity.Add(matchData.Groups["humidity"].Value);

                            matchData = matchData.NextMatch();
                        }

                        allData.Add(sensorData);
                    }
                }

                stopWatch.Stop();
                
                Console.WriteLine($"Elapsed Time for parsing and format conversion: {stopWatch.ElapsedMilliseconds} ms");

                stopWatch.Restart();
                
                // Write to a JSON File
                File.WriteAllText(fileName + ".json",
                        JsonConvert.SerializeObject(allData, Formatting.Indented));

                Console.WriteLine($"Elapsed Time write to file: {stopWatch.ElapsedMilliseconds} ms");
            }
            Console.WriteLine("Done.");
        }

        [TestMethod]
        public void ProcessFile()
        {
            string fileName = _fileDirectory + @"\" + "sensordata_small.txt";

            ParseSensorData(fileName);
        }
    }
}
