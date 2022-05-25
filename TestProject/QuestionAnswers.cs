﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace TestProject
{
    [TestClass]
    public class QuestionAnswers
    {
        /*
         * Objective: Remove Embedded Comma
         * Question and pattern by Minhchau:
            https://www.udemy.com/course/learning-regular-expression-with-net/learn/#questions/17551850/
        */

        [TestMethod]
        public void RemoveEmbeddedComma()
        {
            // rewrote the pattern
            //  using multiline and make sure double quotes
            //  are escaped properly
            string pattern = @"(?m)(""[^"",]+),([^""]+"")";

            Console.WriteLine(pattern);

            string sub = "$1$2";
            string directory = @"C:\Git\RegexDotNet\Data\QuestionAnswerData";
            string fileName = @"embeddedQuotes.csv";

            var fileContents = System.IO.File.ReadAllText(directory + @"\" + fileName);

            Console.WriteLine("*** Original Content ***");
            Console.WriteLine(fileContents);
            fileContents = Regex.Replace(fileContents, pattern, sub);
            Console.WriteLine("*** Modified Content ***");
            Console.WriteLine(fileContents);
        }
    }
}
