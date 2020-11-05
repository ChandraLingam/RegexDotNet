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
    }
}
