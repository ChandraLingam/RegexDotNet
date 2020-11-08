using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestProject
{
    [TestClass]
    public class PerformanceStaticMethod
    {
        /* Timeout to stop long running search */
        [TestMethod]        
        public void TimeoutExample()
        {
            // Pattern takes over 10 seconds
            // Set a timeout for 5 seconds

            string pattern = @"^(\w*)*$";            
            string text = "1234567890123456789012aaa!";

            try
            {
                Match match = Regex.Match(text, 
                                          pattern, 
                                          RegexOptions.None, 
                                          new TimeSpan(0, 0, 5));

                Console.WriteLine("Pattern: {0}", pattern);
                Console.WriteLine("Text: {0}", text);

                if (match.Success)
                {
                    Console.WriteLine("Matching value: {0}  Index: {1}  Length: {2}", match.Value, match.Index, match.Length);
                }
                else
                {
                    Console.WriteLine("Match Failed");
                }
            }
            catch(Exception err)
            {
                Console.WriteLine($"{err.Message}");
            }            
        }

        [TestMethod]
        public void StaticMethodRepetition()
        {
            // Invoke match method with same pattern and text for 10,000 times
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"Timestamp=20160502";
            int failCount = 0;
            int passCount = 0;

            for (int i = 0; i < 10000; i++)
            {
                var match = Regex.Match(text, pattern);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }


        [TestMethod]
        public void StaticMethodRepetitionCompiled()
        {
            // Invoke match method with same pattern and text for 10,000 times
            // Pattern is compiled

            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"Timestamp=20160502";
            int failCount = 0;
            int passCount = 0;

            for (int i = 0; i < 10000; i++)
            {
                var match = Regex.Match(text, pattern, RegexOptions.Compiled);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        // Specify Pattern Count
        public void VariablePatterns(int patternCount = 15, RegexOptions regexOptions = RegexOptions.None)
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<string> patterns = new List<string>();

            Console.WriteLine($"Pattern Count: {patternCount} Cache Size: {Regex.CacheSize}");

            for (int i = 0; i < patternCount; i++)
            {
                patterns.Add(patternPrefix + i);
            }

            string text = @"Timestamp=20160501";
            int failCount = 0;
            int passCount = 0;

            for (int i = 0; i < 10000; i++)
            {
                int selected = i % patternCount;
                var match = Regex.Match(text + selected, patterns[selected], regexOptions);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine($"  Pass: {passCount}, Fail: {failCount}");
        }

        [TestMethod]
        public void StaticMethodVariablePatterns()
        {
            /*  
             *  Vary the number of patterns. 
             *  
             *  Default Cache Size is 15 patterns
             *  
             *  Test 1: 10 Patterns
             *  Test 2: 15 Patterns
             *  Test 3: 20 Patterns
             *  Test 4: 500 Patterns
             */

            int[] numberOfPatterns = { 10, 15, 20, 500 };

            Stopwatch stopWatch = new Stopwatch();

            foreach (int count in numberOfPatterns)
            {
                Console.WriteLine("****");

                stopWatch.Start();
                VariablePatterns(count);
                stopWatch.Stop();

                Console.WriteLine($"Elapsed Time: {stopWatch.ElapsedMilliseconds} ms");
                Console.WriteLine();

                stopWatch.Reset();
            }
        }

        [TestMethod]
        public void StaticMethodVariablePatternsCompiled()
        {
            /*  
             *  Vary the number of patterns and compile patterns
             *  
             *  Default Cache Size is 15 patterns
             *  
             *  Test 1: 10 Patterns
             *  Test 2: 15 Patterns
             *  Test 3: 20 Patterns
             *  Test 4: 500 Patterns
             */

            int[] numberOfPatterns = { 10, 15, 20, 500 };

            Stopwatch stopWatch = new Stopwatch();

            foreach (int count in numberOfPatterns)
            {
                Console.WriteLine("****");

                stopWatch.Start();
                VariablePatterns(count, RegexOptions.Compiled);
                stopWatch.Stop();

                Console.WriteLine($"Elapsed Time: {stopWatch.ElapsedMilliseconds} ms");
                Console.WriteLine();

                stopWatch.Reset();
            }
        }

        [TestMethod]
        public void StaticMethodAdjustCache()
        {
            /*  
             *  Vary the cache size to match the number of patterns. 
             *  
             *  Test 1: 10 Patterns
             *  Test 2: 15 Patterns
             *  Test 3: 20 Patterns
             *  Test 4: 500 Patterns
             */

            int[] numberOfPatterns = { 10, 15, 20, 500 };

            Stopwatch stopWatch = new Stopwatch();

            foreach (int count in numberOfPatterns)
            {
                // Adjust cache size
                Regex.CacheSize = count;

                Console.WriteLine("****");

                stopWatch.Start();
                VariablePatterns(count, RegexOptions.Compiled);
                stopWatch.Stop();

                Console.WriteLine($"Elapsed Time: {stopWatch.ElapsedMilliseconds} ms");
                Console.WriteLine();

                stopWatch.Reset();
            }
        }
    }
}
