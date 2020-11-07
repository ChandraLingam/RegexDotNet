using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace TestProject
{
    [TestClass]
    public class HealthCareSolution
    {
        // TODO: Update path to point to your directory
        string _fileDirectory = @"C:\Git\RegexDotNet\Data\HealthData";


        public void ParseHealthDataTable(string fileName)
        {
            // Capture row                                  
            string PATTERN_ROW = @"(?:<tr\s*/>)|(?:<tr\b[^>]*>(?<arow>.+?)</tr>)";

            // Capture column/cell
            string PATTERN_CELL = @"<(?:th|td)\s*/>|<(?<element>td|th)\b[^>]*>(?<col>.*?)</\k<element>>";

            // Cleanup
            string PATTERN_CLEANUP = @"\s+|&#160;";

            string htmlContent = File.ReadAllText(fileName);

            //Replaces all extra spaces, new lines, tabs etc with a single space.
            htmlContent = Regex.Replace(htmlContent, PATTERN_CLEANUP, " ");

            // Process each row in the table
            Match row = Regex.Match(htmlContent, PATTERN_ROW);

            using (StreamWriter writer = new StreamWriter(fileName + ".csv", false))
            {
                // Iterate all rows
                while (row.Success)
                {
                    if (!row.Groups["arow"].Success)
                    {
                        // skip empty rows
                        continue;
                    }

                    List<string> rowData = new List<string>();

                    string arow = row.Groups["arow"].Value;

                    // Process all columns in a row
                    Match column = Regex.Match(arow, PATTERN_CELL);

                    while (column.Success)
                    {
                        if (column.Groups["col"].Success)
                        {
                            // column value - replace embedded comma
                            rowData.Add(column.Groups["col"].Value.Replace(',', ' '));
                        }
                        else
                        {
                            // Empty Column
                            rowData.Add("");
                        }
                        column = column.NextMatch();
                    }

                    writer.WriteLine(string.Join(",", rowData));
                    row = row.NextMatch();
                }
            }
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
