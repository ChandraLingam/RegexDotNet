using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Retired
{
    public class SensorData
    {
        public DateTime dateTime;
        public Dictionary<string, List<short?>> sensorValue = new Dictionary<string, List<short?>>();
    }

    [TestClass]
    public class SensorDataExample
    {
        [TestMethod]
        public void ParseSensorDataTest()
        {
            Console.WriteLine("Started...");
            Console.WriteLine(System.Environment.CurrentDirectory);

            string file = @"C:\Git\RegexDotNet\Data\SensorData\sensordata_365.txt";
            string jsonOutputFile = @"C:\Git\RegexDotNet\Data\SensorData\sensordata_365.txt.json";

            string PATTERN_HEADER = @"^(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string PATTERN_TEMP_HUM = @"(?<temperature>\d{4}|NNNN)(?<humditiy>\d{3}|NNN)";
           
            Regex regexSensorValue = new Regex(PATTERN_TEMP_HUM);
            List<string> sensorParameterNames = new List<string>();

            foreach (string name in regexSensorValue.GetGroupNames())
            {
                if (name.Equals("0"))
                    continue;
                else
                    sensorParameterNames.Add(name);
            }

            using (StreamReader rdr = new StreamReader(file))
            {
                List<SensorData> list = new List<SensorData>();
                while (!rdr.EndOfStream)
                {
                    SensorData sd = new SensorData();
                    string currentLine = rdr.ReadLine();

                    Match m = Regex.Match(currentLine, PATTERN_HEADER);

                    if (m.Success)
                    {
                        sd.dateTime = new DateTime(
                            int.Parse(m.Groups["year"].Value),
                            int.Parse(m.Groups["month"].Value),
                            int.Parse(m.Groups["day"].Value));

                        foreach (string parameterName in sensorParameterNames)
                        {
                            sd.sensorValue[parameterName] = new List<short?>();
                        }

                        m = regexSensorValue.Match(currentLine, m.Value.Length);

                        short value = 0;
                        while (m.Success)
                        {
                            foreach (string parameterName in sensorParameterNames)
                            {
                                if (short.TryParse(m.Groups[parameterName].Value, out value))
                                    sd.sensorValue[parameterName].Add(value);
                                else
                                    sd.sensorValue[parameterName].Add(null);
                            }

                            m = m.NextMatch();
                        }

                        list.Add(sd);
                    }
                }

                using (StreamWriter writer = new StreamWriter(jsonOutputFile, false))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, list);
                }
            }
            Console.WriteLine("Done.");
        }
    }
}
