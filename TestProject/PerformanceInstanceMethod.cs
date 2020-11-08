using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestProject
{
    [TestClass]
    public class PerformanceInstanceMethod
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
                // Specify timeout during object construction
                // Object is tied to a specific pattern
                Regex regex = new Regex(pattern, 
                                        RegexOptions.None,
                                        new TimeSpan(0, 0, 5));

                Match match = regex.Match(text);

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
        public void InstanceMethodRepetition()
        {
            // Invoke match method with same pattern and text for 10,000 times
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"Timestamp=20160502";
            int failCount = 0;
            int passCount = 0;

            Regex regex = new Regex(pattern);

            for (int i = 0; i < 10000; i++)
            {
                var match = regex.Match(text);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        [TestMethod]
        public void InstanceMethodRepetitionCompiled()
        {
            // Invoke match method with same pattern and text for 10,000 times
            // Pattern is compiled

            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"Timestamp=20160502";
            int failCount = 0;
            int passCount = 0;

            Regex regex = new Regex(pattern, RegexOptions.Compiled);

            for (int i = 0; i < 10000; i++)
            {
                var match = regex.Match(text);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        [TestMethod]
        public void InstanceMethodRepetitionAntiPattern()
        {
            // Antipattern 
            // Regex object is created inside a loop
            // In addition, it gets compiled

            // Correct approach : reuse same instance

            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"Timestamp=20160502";
            int failCount = 0;
            int passCount = 0;
            
            for (int i = 0; i < 10000; i++)
            {
                Regex regex = new Regex(pattern, RegexOptions.Compiled);
                var match = regex.Match(text);

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
            string text = @"Timestamp=20160501";
            int failCount = 0;
            int passCount = 0;

            List<Regex> patterns = new List<Regex>();

            Console.WriteLine($"Pattern Count: {patternCount}");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            int instanceCount = 0;
            for (int i = 0; i < patternCount; i++)
            {
                Regex regex = new Regex(patternPrefix + i, regexOptions);

                // To prevent lazy compilation
                //  Making first call after creating the object
                if (regex.Match(text + i).Success)
                    instanceCount++;

                patterns.Add(regex);
            }
            stopWatch.Stop();
            Console.WriteLine($"  Instance Count:{instanceCount}");
            Console.WriteLine($"  Instance Creation Time: {stopWatch.ElapsedMilliseconds} ms");

            stopWatch.Restart();
            for (int i = 0; i < 10000; i++)
            {
                int selected = i % patternCount;
                var match = patterns[selected].Match(text + selected);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }
            stopWatch.Stop();

            Console.WriteLine($"  Method invocation Time: {stopWatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"  Pass: {passCount}, Fail: {failCount}");
        }

        [TestMethod]
        public void InstanceMethodVariablePatterns()
        {
            /*  
             *  Vary the number of patterns. 
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
        public void InstanceMethodVariablePatternsCompiled()
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
                VariablePatterns(count, RegexOptions.Compiled);
                stopWatch.Stop();

                Console.WriteLine($"Elapsed Time: {stopWatch.ElapsedMilliseconds} ms");
                Console.WriteLine();

                stopWatch.Reset();
            }
        }
    }
}
