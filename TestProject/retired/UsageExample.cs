using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace TestProject
{
    [TestClass]
    public class UsageExample
    {
        [TestMethod]
        public void ValidationExample1()
        {
            // Purpose: Verify if the given text is made up of digits
            string pattern = @"^\d+$";            
            string text = "1234";
            string invalidText = "12A34";

            bool result = Regex.IsMatch(text, pattern);

            Console.WriteLine("Pattern: {0}", pattern);
            Console.WriteLine("Text: {0}, Valid: {1}", text, result);
            
            result = Regex.IsMatch(invalidText, pattern);
            Console.WriteLine("Text: {0}, Valid: {1}", invalidText, result);
        }

        [TestMethod]
        public void ValidationExample2()
        {
            // Purpose: Verify if the given text is made up of digits
            string pattern = @"^\d+$";
            string[] positiveTest = { "123456", "456", "321082", "0820102" };
            string[] negativeTest = { "ABCD", "A1234", "1234AB", "  123", "321  ", "  111   ", "123 4567", "123\n456" };

            Console.WriteLine("***Positive Test:");
            foreach (string word in positiveTest)
            {

                bool result = Regex.IsMatch(word, pattern);
                Console.WriteLine("Text:{0}, Match:{1}", word, result);
                Assert.AreEqual(true, result,
                    string.Format("Expected successful match but failed.  word: {0} pattern:{1}", word, pattern));
            }

            Console.WriteLine("***Negative Test:");
            foreach (string word in negativeTest)
            {
                bool result = Regex.IsMatch(word, pattern);
                Console.WriteLine("Text:{0}, Match:{1}", word, result);
                Assert.AreEqual(false, result,
                    string.Format("Expected a failed match but succeeded.  word: {0} pattern:{1}", word, pattern));
            }
        }

        [TestMethod]
        public void MatchExample()
        {
            // Purpose: Extract 5 digit postal codes
            string pattern = @"\b\d{5}\b";
            string text = "NY Postal Codes are 10001, 10002, 10003, 10004";


            Match match = Regex.Match(text, pattern);

            Console.WriteLine("Pattern: {0}", pattern);
            Console.WriteLine("Text: {0}", text);

            while (match.Success)
            {
                Console.WriteLine("Matching value: {0}  Index: {1}  Length: {2}", 
                    match.Value, match.Index, match.Length);

                match = match.NextMatch();
            }
        }

        [TestMethod]
        public void GroupExample()
        {          
            string pattern = 
             @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})(?<hour>\d{2})(?<minute>\d{2})";

            string text = "Timestamp=201605021445";

            Match match = Regex.Match(text, pattern);

            Console.WriteLine("Pattern: {0}", pattern);
            Console.WriteLine("Text: {0}", text);

            if (match.Success)
            {
                Console.WriteLine("Matching value: {0}  Index: {1}  Length: {2}", 
                    match.Value, match.Index, match.Length);

                Console.WriteLine("  Year: {0}, Matched? {1}", 
                    match.Groups["year"].Value, match.Groups["year"].Success);
                Console.WriteLine("  Month: {0}", match.Groups["month"].Value);
                Console.WriteLine("  Day: {0}", match.Groups["day"].Value);
                Console.WriteLine("  Hour: {0}", match.Groups["hour"].Value);
                Console.WriteLine("  Minute: {0}", match.Groups["minute"].Value);
                Console.WriteLine("  seconds: {0}, Matched? {1}",
                    match.Groups["seconds"].Value, match.Groups["seconds"].Success);
            }
        }

        [TestMethod]
        public void GroupByNumberExample()
        {
            string pattern =
             @"(\d{4})(\d{2})(\d{2})";

            string text = "Timestamp=20160502";

            Match match = Regex.Match(text, pattern);

            Console.WriteLine("Pattern: {0}", pattern);
            Console.WriteLine("Text: {0}", text);

            if (match.Success)
            {
                Console.WriteLine("Matching value: {0}  Index: {1}  Length: {2}",
                    match.Value, match.Index, match.Length);

                for (int i = 0; i < match.Groups.Count; i++)
                {
                    Console.WriteLine("  Group Index:{0}, Value:{1}, Match?:{2}", 
                        i, match.Groups[i].Value, match.Groups[i].Success);
                }                
            }
        }

        [TestMethod]
        public void GroupByNameExample()
        {
            string pattern =
             @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";

            string text = "Timestamp=20160502";

            Match match = Regex.Match(text, pattern);

            Console.WriteLine("Pattern: {0}", pattern);
            Console.WriteLine("Text: {0}", text);

            if (match.Success)
            {
                Console.WriteLine("Matching value: {0}  Index: {1}  Length: {2}",
                    match.Value, match.Index, match.Length);

                Console.WriteLine("  Group 'year' Value:{0}, Match?:{1}",
                        match.Groups["year"].Value, match.Groups["year"].Success);
                Console.WriteLine("  Group 'month' Value:{0}, Match?:{1}",
                        match.Groups["month"].Value, match.Groups["month"].Success);
                Console.WriteLine("  Group 'day' Value:{0}, Match?:{1}",
                        match.Groups["day"].Value, match.Groups["day"].Success);
            }
        }

        [TestMethod]
        public void ReplaceExample()
        {
            string pattern = @"(?<value>\d+(,\d{3})*(\.\d{2})?)\s+dollar(s)?";
            string replacePattern = @"**USD ${value}**";

            string text = @"Widget Unit cost: 12,000.56 dollars
Taxes: 234.00 dollars
Total: 12,234.56 dollars";

            string newText = Regex.Replace(text, pattern, replacePattern);

            Console.WriteLine("Pattern: {0}", pattern);
            Console.WriteLine("Replace Pattern: {0}", replacePattern);

            Console.WriteLine("----Text:");
            Console.WriteLine(text);

            Console.WriteLine("----New Text:");
            Console.WriteLine(newText);
        }

        public static string CelsiusToFahrenheit(Match m)     
        {
            float degCelsius = float.Parse(m.Groups["celsius"].Value);
            float degF = 32.0f + (degCelsius * 9.0f / 5.0f);

            return degF + @"°F";
        }

        [TestMethod]
        public void ReplaceExample2()
        {
            string pattern = @"(?<celsius>\d+)\u00B0C";
             
            string text = @"Today's temperature is 32°C";

            string newText = Regex.Replace(text, pattern, new MatchEvaluator(CelsiusToFahrenheit));

            Console.WriteLine("Pattern: {0}", pattern);

            Console.WriteLine("----Text:");
            Console.WriteLine(text);

            Console.WriteLine("----New Text:");
            Console.WriteLine(newText);
        }

        [TestMethod]
        public void SplitExample()
        {
            //string pattern = @"\d+\.\s*";
            string pattern = @"\d+\p{P}\s*";
            string text =
            @"Here is the shopping list:  1.     Cilantro	2) Carrot	3. Milk		4. Eggs";

            string[] splitText = Regex.Split(text, pattern);

            Console.WriteLine("Pattern: {0}", pattern);

            Console.WriteLine("----Text:");
            Console.WriteLine(text);

            Console.WriteLine("----Split Text:");
            foreach (string s in splitText)
                Console.WriteLine("**{0}**", s);
        }

        

        [TestMethod]
        public void VerbatimStringExample()
        {
            string[] regularString = { "\\d\t\\d", "\\d\x09\\d", "Hello\"World", "\\\\server\\share\\file.txt", "Line1\nLine2"};
            string[] verbatimString = { @"\d\t\d", @"\d\x09\d", @"Hello ""World", @"\\server\share\file.txt", @"Line1\nLine2"};

            for(int i = 0; i < regularString.Length; i++)
            {
                Console.WriteLine("****");
                Console.WriteLine("Regular: {0}", regularString[i]);
                Console.WriteLine("Verbatim: {0}", verbatimString[i]);
            }           
        }

        [TestMethod]
        public void RightToLeftWordsTest()
        {
            var pattern = @"\w+";
            var text = "one two three four";

            Console.WriteLine("Looking for pattern: {0} in text: {1}", pattern, text);

            var match = Regex.Match(text, pattern, RegexOptions.RightToLeft);
            Console.WriteLine("Reversed Words:");

            while (match.Success)
            {
                Console.Write(" {0} ", match.Value);

                match = match.NextMatch();
            }
            Console.WriteLine();
        }

        [TestMethod]
        public void RightToLeftReverseTest()
        {
            var pattern = @".";
            var text = "one two three four!";

            Console.WriteLine("Looking for pattern: {0} in text: {1}", pattern, text);
            
            var match = Regex.Match(text, pattern, RegexOptions.RightToLeft);

            Console.WriteLine("Reverse String");
            while (match.Success)
            {
                Console.Write(match.Value);
                match = match.NextMatch();
            }
        }

        [TestMethod]
        public void RightToLeftReverseMultiLineTest()
        {
            var pattern = @".";
            var text = "one two three four\r\nfive six seven eight";

            Console.WriteLine("Looking for pattern: {0} in text: {1}", pattern, text);

            var match = Regex.Match(text, pattern, RegexOptions.RightToLeft | RegexOptions.Multiline);

            Console.WriteLine("Reverse String");
            while (match.Success)
            {
                Console.Write(match.Value);
                match = match.NextMatch();
            }
        }        
    }
}