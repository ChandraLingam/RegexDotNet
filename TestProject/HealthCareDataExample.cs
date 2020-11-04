using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace TestProject
{
    [TestClass]
    public class HealthCareDataExample
    {
        public void ParseHealthDataTable(string fileName)
        {
            string htmlContent = File.ReadAllText(fileName);

            Console.WriteLine("Started...");
            
            //Replaces all extra spaces, new lines, tabs etc with a single space.
            htmlContent = Regex.Replace(htmlContent, @"\s+|&#160;", " ");

            // Match a header or body row
            // Handles empty rows
            string PATTERN_ROW = @"<tr\s*/>|<tr\b[^>]*>(?<arow>.+?)</tr>";
            
            // Each cell is made up of td or th
            // Backreference to ensure td pair or th pair is matched.
            // Handles empty cells
            string PATTERN_CELLS = @"<(td|th)\s*/>|<(?<element>td|th)\b[^>]*>(?<col>.+?)</\k<element>>";

            Match row = Regex.Match(htmlContent, PATTERN_ROW);

            StringBuilder sb = new StringBuilder();
            using (StreamWriter writer = new StreamWriter(fileName + ".csv", false))
            {
                while (row.Success)
                {
                    if (!row.Groups["arow"].Success)
                    {
                        //Empty row
                        continue;
                    }

                    sb.Clear();

                    string arow = row.Groups["arow"].Value;
                    // Console.WriteLine(arow);
                    Match cols = Regex.Match(arow, PATTERN_CELLS);

                    while (cols.Success)
                    {
                        if (cols.Groups["col"].Success)
                        {
                            sb.Append(string.Format("{0},", cols.Groups["col"].ToString().Replace(',',' ')));
                        }
                        else
                        {
                            // Empty Column
                            sb.Append(",");
                        }
                        cols = cols.NextMatch();
                    }

                    writer.WriteLine(sb.ToString(0, sb.Length - 1));
                    row = row.NextMatch();
                }
            }
        }

        [TestMethod]
        public void ProcessFiles()
        {
            string[] fileList = {
                @"C:\RegexCourse\Data\HealthData\problems.html",
                @"C:\RegexCourse\Data\HealthData\labresults.html"};
            foreach (string file in fileList)
            {
                Console.WriteLine("****{0}", file);
                ParseHealthDataTable(file);
            }
        }
    }
}
