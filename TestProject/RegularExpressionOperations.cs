using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;


namespace TestProject
{
    /*
     * In this section, we explore .NET regex class, operations and purpose
     */

    [TestClass]
    public class RegularExpressionOperations
    {        
        [TestMethod]
        public void PatternRepresentation()
        {
            // Literal String and Regular String
            // regular string 
            //  -> embedded special characters are interpreted to escape, 
            //     you need add a backslash "\"

            string regularString = "a\\tb"; 
            string literalString = @"a\tb";

            Console.WriteLine($"regular string: {regularString}");
            Console.WriteLine(); 
            Console.WriteLine($"literal string: {literalString}");
            Console.WriteLine();
        }


        [TestMethod]
        public void IsMatch()
        {
            // Check if there is a match

            // \d = digit. + = one or more.  This pattern matches one or more digits
            string pattern = @"\d+";
            string text = "42 is my lucky number";

            bool result = Regex.IsMatch(text, pattern);

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");

            Console.WriteLine();
            Console.WriteLine($"Is there a match ? {result}");
        }

        public bool IsInteger(string text)
        {
            /* 
             * Pattern 1                
             *  \d = digit                
             *  \d+ = one or more digits
             *  pattern = \d+
             *  
             *  
             * Pattern 2
             *  start of string or line. followed by one or more digits. followed by end of string or line
             *  ^ = start of string or line
             *  $ = end of string or line
             *  pattern = ^\d+$
             *  
            */

            string pattern = @"\d+";

            return Regex.IsMatch(text, pattern);
        }

        [TestMethod]
        public void IntegerTest()
        {
            string text = "1234";
                        
            Console.WriteLine($"Is {text} an integer? {IsInteger(text)}");
            Console.WriteLine();

            text = "abcd";
            Console.WriteLine($"Is {text} an integer? {IsInteger(text)}");
        }

        [TestMethod]
        public void IntegerUnitTest()
        {
            string[] pass_list = { "123", "456", "900", "0991" };
            string[] fail_list = { "a123", "124a", "1 2 3", "1\t2", " 12", "45 " };

            // misclassification
            //   integer misclassified as a non-integer
            List<string> false_negative = new List<string>();
            //   non-integer misclassified as an integer
            List<string> false_positive = new List<string>();

            foreach (string text in pass_list)
            {
                if (!IsInteger(text))
                    false_negative.Add(text);
            }

            foreach (string text in fail_list)
            {
                if (IsInteger(text))
                    false_positive.Add(text);
            }

            if (false_negative.Count > 0)
                Console.WriteLine($"False Negative: {string.Join(",", false_negative.ToArray())}");
            else
                Console.WriteLine("False Negative: None");

            Console.WriteLine();

            if (false_positive.Count > 0)
                Console.WriteLine($"False Positive: {string.Join(",", false_positive.ToArray())}");
            else
                Console.WriteLine("False Positive: None");
        }

        [TestMethod]
        public void FirstMatch()
        {
            // Retrieve matching substring

            // \d = digit. + = one or more.  This pattern matches one or more digits
            string pattern = @"\d+";
            string text = "my lucky number is 42";

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();

            Match match = Regex.Match(text, pattern);

            if (match.Success)
            {
                Console.WriteLine
                    ($"Found a match: {match.Value} at index: {match.Index} length: {match.Length}");
            }
            else
                Console.WriteLine("No Match");
        }

        [TestMethod]
        public void IterateMatch()
        {
            // Iterate all matches

            // \d = digit. + = one or more.  This pattern matches one or more digits
            string pattern = @"\d+";
            string text = "NY Postal Codes are 10001, 10002, 10003, 10004";

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();

            Match match = Regex.Match(text, pattern);

            while (match.Success)
            {
                Console.WriteLine
                    ($"Found a match: {match.Value} at index: {match.Index} length: {match.Length}");

                match = match.NextMatch();
            }
        }

        [TestMethod]
        public void IterateMatches()
        {
            // Iterate all matches

            // \d = digit. + = one or more.  This pattern matches one or more digits
            string pattern = @"\d+";
            string text = "NY Postal Codes are 10001, 10002, 10003, 10004";

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();

            MatchCollection matchCollection = Regex.Matches(text, pattern);

            Console.WriteLine("Lazy Evaluation - one match at a time");
            // Lazy evaluation - scans the text when next match is required
            foreach(Match match in matchCollection)
            {
                Console.WriteLine
                    ($"\tFound a match: {match.Value} at index: {match.Index} length: {match.Length}");
            }
            Console.WriteLine();

            Console.WriteLine("Scans entire text before returning");
            // Count and CopyTo operations will scan the text all at once
            matchCollection = Regex.Matches(text, pattern);

            // finds all matches
            Console.WriteLine($"\tTotal Matches: {matchCollection.Count}");

            Match[] result = new Match[matchCollection.Count];
            matchCollection.CopyTo(result,0);

            foreach (Match match in result)
            {
                Console.WriteLine
                    ($"\tFound a match: {match.Value} at index: {match.Index} length: {match.Length}");
            }
        }

        [TestMethod]
        public void GroupsIndexed()
        {
            // Iterate all matches

            // \d = digit. + = one or more.  This pattern matches one or more digits
            string pattern = @"(\d{4})(\d{2})(\d{2})";
            string text = "Start Date: 20200920";

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();

            Match match = Regex.Match(text, pattern);

            if (match.Success)
            {
                Console.WriteLine
                    ($"Found a match: {match.Value} at index: {match.Index} length: {match.Length}");

                for (int i = 0; i < match.Groups.Count; i++)
                {
                    Console.WriteLine($" Group [{i}]\t{match.Groups[i].Value}\tat index {match.Groups[i].Index}");
                }
            }
        }

        [TestMethod]
        public void GroupsNamed()
        {
            // Iterate all matches

            // \d = digit. + = one or more.  This pattern matches one or more digits
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = "Start Date: 20200920";

            string[] namedGroups = { "year", "month", "day" };

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();

            Match match = Regex.Match(text, pattern);

            if (match.Success)
            {
                Console.WriteLine
                    ($"Found a match: {match.Value} at index: {match.Index} length: {match.Length}");

                foreach (string name in namedGroups)
                {
                    Console.WriteLine
                        ($" Group {name}\t{match.Groups[name].Value}\tat index {match.Groups[name].Index}");
                }
            }
        }

        [TestMethod]
        public void Replace()
        {
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";

            string text = "Start Date: 20200920, End Date: 20210920";

            string replacementPattern = @"${month}-${day}-${year}";

            string newText = Regex.Replace(text, pattern, replacementPattern);

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();
            Console.WriteLine($"Replacemeth Pattern: {replacementPattern}");
            Console.WriteLine();

            Console.WriteLine($"Original Text\t{text}");
            Console.WriteLine();

            Console.WriteLine($"New Text\t{newText}");
            Console.WriteLine();
        }

        public static string FormatDate(Match match)
        {
            DateTime dt = new DateTime(Int32.Parse(match.Groups["year"].Value), 
                                       Int32.Parse(match.Groups["month"].Value),
                                       Int32.Parse(match.Groups["day"].Value));

            return dt.ToString("MMM-dd-yyyy");
        }

        [TestMethod]
        public void ReplaceCustom()
        {
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";

            string text = "Start Date: 20200920, End Date: 20210920";

            string newText = Regex.Replace(text, pattern, new MatchEvaluator(FormatDate));

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();

            Console.WriteLine($"Original Text\t{text}");
            Console.WriteLine();

            Console.WriteLine($"New Text\t{newText}");
            Console.WriteLine();
        }


        [TestMethod]
        public void SplitExample()
        {
            string pattern = @";";
            string text = "a-c;x;y;1";

            string[] splitText = Regex.Split(text, pattern);

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine();
            Console.WriteLine($"Text: {text}");
            Console.WriteLine();

            Console.WriteLine("Split Text:");
            foreach (string s in splitText)
                Console.WriteLine($"  {s}");
        }
    }
}
