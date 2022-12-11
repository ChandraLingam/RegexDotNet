using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TestProject
{
    /// <summary>
    /// Summary description for QuizCodeSnippet
    /// </summary>
    [TestClass]
    public class QuizCodeSnippet
    {
        public QuizCodeSnippet()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void IterateMatch()
        {
            // Iterate all matches
            string pattern = @"";
            string text = "1 123 ABCD123 12345";

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine($"Text: {text}");

            Match match = Regex.Match(text, pattern);

            while (match.Success)
            {
                Console.WriteLine
                    ($"***Found a match: {match.Value} at index: {match.Index} length: {match.Length}");

                match = match.NextMatch();
            }
        }

        [TestMethod]
        public void ManyInputItems()
        {
            // Iterate all matches
            string pattern = @"";
            string[] test_data = { "Av!1","abcd","Av&1","!cA3R"};

            Console.WriteLine($"Pattern: {pattern}");

            foreach (string item in test_data)
            {
                Console.WriteLine($"Text: {item}");
                Match match = Regex.Match(item, pattern);

                if (match.Success)
                {
                    Console.WriteLine
                        ($"***Found a match: {match.Value} at index: {match.Index} length: {match.Length}");                    
                }
            }            
        }

        [TestMethod]
        public void MultiLineTextMatch()
        {
            // Iterate all matches
            string pattern = @"";
            string text = @"exl-accord
ex-accord
se-camry
xle-camry
exl-civic";

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine($"Text: {text}");

            Match match = Regex.Match(text, pattern);

            while (match.Success)
            {
                Console.WriteLine
                    ($"***Found a match: {match.Value} at index: {match.Index} length: {match.Length}");

                match = match.NextMatch();
            }
        }

        [TestMethod]
        public void GroupsIndexed()
        {
            string pattern = @""; // try both the patterns (\w+) and (\w)+
            string text = "apple";

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine($"Text: {text}");

            Match match = Regex.Match(text, pattern);

            if (match.Success)
            {
                Console.WriteLine
                    ($"Found a match: {match.Value} at index: {match.Index} length: {match.Length}");

                for (int i = 1; i < match.Groups.Count; i++)
                {
                    Console.WriteLine($" Group [{i}]\t{match.Groups[i].Value}\tat index {match.Groups[i].Index}");
                }
            }
        }

        [TestMethod]
        public void GroupsNamed()
        {
            string pattern = @"";
            string text = @"chair 4.98
coffee 1.99
fan 10.97
car 11499.59
banana 0.09";

            string[] namedGroups = { "ip", "http", "resource", "bytes", "duration" };

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine($"Text: {text}");

            Match match = Regex.Match(text, pattern);

            while (match.Success)
            {
                Console.WriteLine
                    ($"Found a match: {match.Value} at index: {match.Index} length: {match.Length}");

                foreach (string name in namedGroups)
                {
                    Console.WriteLine
                        ($"  {name}\t{match.Groups[name].Value}\tat index {match.Groups[name].Index}");
                }

                match = match.NextMatch();

                Console.WriteLine();
            }
        }

        [TestMethod]
        public void Replace()
        {
            string pattern = @"";
            string replacementPattern = @"";

            // Change date format 20200920 => 09-20-2020
            string text = @"movie ticket: $15, popcorn: $8
movie ticket: €15, popcorn: €8
movie ticket: ₹15, popcorn: ₹8";

            string newText = Regex.Replace(text, pattern, replacementPattern);

            Console.WriteLine($"Pattern: {pattern}");
            Console.WriteLine($"Replacemeth Pattern: {replacementPattern}");
            
            Console.WriteLine($"Original Text\t{text}");
            Console.WriteLine();

            Console.WriteLine($"New Text\t{newText}");
            Console.WriteLine();
        }
    }
}
