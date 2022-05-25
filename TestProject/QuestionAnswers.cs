using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace TestProject
{
    [TestClass]
    public class QuestionAnswers
    {
        [TestMethod]
        public void RemoveEmbeddedComma()
        {
            string pattern = @"(?m)(""[^"",]+),([^""]+"")";

            Console.WriteLine(pattern);

            string sub = "$1$2";
            string directory = @"C:\Git\RegexDotNet\Data\QuestionAnswerData";
            string fileName = @"embeddedQuotes.csv";

            var fileContents = System.IO.File.ReadAllText(directory + @"\" + fileName);

            Console.WriteLine("***");
            Console.WriteLine(fileContents);
            fileContents = Regex.Replace(fileContents, pattern, sub);
            Console.WriteLine(fileContents);
        }
    }
}
