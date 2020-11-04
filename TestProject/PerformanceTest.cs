using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestProject
{
    [TestClass]
    public class PerformanceTest
    {
        /* Timeout to stop long running search */
        [TestMethod]        
        public void TimeoutExample()
        {
            string pattern = @"^(\w*)*$";
            string text = "1234567890123456789012345!";
            
            Match match = Regex.Match(text, pattern, RegexOptions.None, new TimeSpan(0, 0, 5));

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

        [TestMethod]
        public void StaticMethodCacheSizeTest()
        {
            Console.WriteLine(Regex.CacheSize);
        }

        [TestMethod]
        public void Static_Method_NoCompile_Repetition_Test()
        {
            // New Test
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
        public void Static_Method_Compile_Repetition_Test()
        {
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"Timestamp=20160502";
            int failCount = 0;
            int passCount = 0;
            for (int i = 0; i < 10000; i++)
            {
                var match = Regex.Match(text, pattern,RegexOptions.Compiled);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }


        [TestMethod]
        public void StaticMethod10Patterns()
        {
            VaryPatterns(10);   
        }

        [TestMethod]
        public void StaticMethod500Patterns()
        {
            VaryPatterns(500);
        }

        [TestMethod]
        public void StaticMethod10PatternsCompiled()
        {
            VaryPatternsCompiled(10);
        }

        [TestMethod]
        public void StaticMethod500PatternsCompiled()
        {
            VaryPatternsCompiled(500);
        }

        [TestMethod]
        public void InstanceMethod10Patterns()
        {
            VaryPatternsRegexObject(10);
        }

        [TestMethod]
        public void InstanceMethod500Patterns()
        {
            VaryPatternsRegexObject(500);
        }

        [TestMethod]
        public void InstanceMethod10PatternsCompiled()
        {
            VaryPatternsRegexObjectCompiled(10);
        }

        [TestMethod]
        public void InstanceMethod500PatternsCompiled()
        {
            VaryPatternsRegexObjectCompiled(500);
        }


        // Specify Pattern Count
        public void VaryPatterns(int patternCount = 15)
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<string> patterns = new List<string>();

            Console.WriteLine("Pattern Count: {0}", patternCount);

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
                var match = Regex.Match(text + selected, patterns[selected]);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        // Specify Pattern Count
        public void VaryPatternsCompiled(int patternCount = 15)
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<string> patterns = new List<string>();

            Console.WriteLine("Pattern Count: {0}", patternCount);

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
                var match = Regex.Match(text + selected, patterns[selected], RegexOptions.Compiled);

                if (match.Success)
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }


        // Specify Pattern Count
        public void VaryPatternsRegexObject(int patternCount = 15)
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<Regex> patterns = new List<Regex>();

            Console.WriteLine("Pattern Count: {0}", patternCount);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < patternCount; i++)
            {
                patterns.Add(new Regex(patternPrefix + i));
            }

            stopWatch.Stop();
            Console.WriteLine("Instance Creation Time: {0} ms", stopWatch.ElapsedMilliseconds);

            string text = @"Timestamp=20160501";
            int failCount = 0;
            int passCount = 0;

            stopWatch.Start();
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
            Console.WriteLine("Method invocation Time: {0} ms", stopWatch.ElapsedMilliseconds);
            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        // Specify Pattern Count
        public void VaryPatternsRegexObjectCompiled(int patternCount = 15)
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<Regex> patterns = new List<Regex>();

            Console.WriteLine("Pattern Count: {0}", patternCount);
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < patternCount; i++)
            {
                patterns.Add(new Regex(patternPrefix + i, RegexOptions.Compiled));
            }

            stopWatch.Stop();
            Console.WriteLine("Compilation Time: {0} ms", stopWatch.ElapsedMilliseconds);

            string text = @"Timestamp=20160501";
            int failCount = 0;
            int passCount = 0;

            stopWatch.Start();
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
            Console.WriteLine("Method invocation Time: {0} ms", stopWatch.ElapsedMilliseconds);

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* Single Static Pattern. 1000 iteration - Good performance*/
        [TestMethod]
        public void StaticMethodRepetitionTest()
        {
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;
            for(int i = 0; i < 1000; i++)
            {
                if (Regex.IsMatch(text, pattern))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* Static 15 Pattern. 1000 iteration - Good performance*/
        [TestMethod]
        public void StaticMethod15PatternRepetitionTest()
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<string> patterns = new List<string>();
            int patternCount = 15;
            
            for (int i = 0; i < patternCount; i++)
            {
                patterns.Add(patternPrefix + i);
            }

            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;

            for (int i = 0; i < 1000; i++)
            {
                int selected = i % patternCount;
                if (Regex.IsMatch(text + selected, patterns[selected]))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* Static 15 Pattern Compiled. 1000 iteration - Good performance. Within cache limit */
        [TestMethod]
        public void StaticMethod15CompiledPatternRepetitionTest()
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<string> patterns = new List<string>();
            int patternCount = 15;

            for (int i = 0; i < patternCount; i++)
            {
                patterns.Add(patternPrefix + i);
            }

            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;

            for (int i = 0; i < 1000; i++)
            {
                int selected = i % patternCount;
                if (Regex.IsMatch(text + selected, patterns[selected], RegexOptions.Compiled))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* Static 20 Pattern. 1000 iteration - Good performance; intrepreted code*/
        [TestMethod]
        public void StaticMethod20PatternRepetitionTest()
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<string> patterns = new List<string>();
            int patternCount = 20;

            for (int i = 0; i < patternCount; i++)
            {
                patterns.Add(patternPrefix + i);
            }

            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;

            for (int i = 0; i < 1000; i++)
            {
                int selected = i % patternCount;
                if (Regex.IsMatch(text + selected, patterns[selected]))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* Static 20 Pattern Compiled. 1000 iteration - Bad performance; LRU is kicking out compiled patterns*/
        [TestMethod]
        public void StaticMethod20CompiledPatternRepetitionTest()
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            List<string> patterns = new List<string>();
            int patternCount = 20;
            //Regex.CacheSize = 20;

            for (int i = 0; i < patternCount; i++)
            {
                patterns.Add(patternPrefix + i);
            }

            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;

            for (int i = 0; i < 1000; i++)
            {
                int selected = i % patternCount;
                if (Regex.IsMatch(text + selected, patterns[selected], RegexOptions.Compiled))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* Single Instance 1 pattern. 1000 iterations - good performance*/
        [TestMethod]
        public void InstanceMethodRepetitionTest()
        {
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;

            Regex r = new Regex(pattern);

            for (int i = 0; i < 1000; i++)
            {                
                if (r.IsMatch(text))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* Separate Compiled instance for every invocation. 1 pattern. 
         * 1000 iterations - bad performance*/
        [TestMethod]
        public void InstanceMethodCompiledBadRepetitionTest()
        {
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;


            for (int i = 0; i < 1000; i++)
            {
                Regex r = new Regex(pattern, RegexOptions.Compiled);
                if (r.IsMatch(text))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* One Compiled instance 1 pattern. 
        * 1000 iterations - Good performance*/
        [TestMethod]
        public void InstanceMethodCompiledGoodRepetitionTest()
        {
            string pattern = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;
            Regex r = new Regex(pattern, RegexOptions.Compiled);

            for (int i = 0; i < 1000; i++)
            {
                if (r.IsMatch(text))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }

        /* One Compiled instance per pattern. 20 patterns
        * 1000 iterations - Good performance*/
        [TestMethod]
        public void InstanceMethod20PatternCacheRepetitionTest()
        {
            string patternPrefix = @"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})";
            Dictionary<string, Regex> rc = new Dictionary<string, Regex>();

            List<string> patterns = new List<string>();
            int patternCount = 15;

            for (int i = 0; i < patternCount; i++)
            {
                rc.Add(patternPrefix + i, new Regex (patternPrefix + i, RegexOptions.Compiled));
            }

            string text = @"20160501";
            int failCount = 0;
            int passCount = 0;

            for (int i = 0; i < 1000; i++)
            {
                int selected = i % patternCount;
                if (rc[patternPrefix + selected].IsMatch(text + selected))
                    passCount++;
                else
                    failCount++;
            }

            Console.WriteLine("PassCount:{0}, FailCount:{1}, Total:{2}", passCount, failCount, passCount + failCount);
        }
    }
}
