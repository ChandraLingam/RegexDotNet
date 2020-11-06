using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;


namespace TestProject
{
    // TODO: Implement Code
    /*
     * In this section, we explore .NET regex class, operations and purpose
     */

    [TestClass]
    public class RegularExpressionOperationsShell
    {        
        [TestMethod]
        public void PatternRepresentation()
        {
            // Always use a literal string for regex pattern

            // Literal String and Regular String
            // regular string 
            //  -> embedded special characters are interpreted to escape, 
            //     you need add a backslash "\"
        }

        [TestMethod]
        public void IsMatch()
        {
            // Regex.IsMatch - Check if there is a match
        }

        // Input Validation - Check if a text is integer
        // Write Unit Tests with positive and negative test cases
        public bool IsInteger(string text)
        {
            // Using regex,  implement logic to check if the 
            // given text is an integer
            // use Regex.IsMatch

            return false;
        }

        [TestMethod]
        public void FirstMatch()
        {
            // Use Regex.Match to retrieve matching substring
        }

        [TestMethod]
        public void IterateMatch()
        {
            // Use Regex.Match to iterate all matches
        }

        [TestMethod]
        public void IterateMatches()
        {
            // Use Regex.Matches to iterate all matches
        }

        [TestMethod]
        public void GroupsIndexed()
        {
            // Access Group using index
        }

        [TestMethod]
        public void GroupsNamed()
        {
            // Access Group using name
        }

        [TestMethod]
        public void Replace()
        {
            // Use Regex.Replace method to find and replace
            // Reformat date YYYYMMDD => MM-DD-YYYY
        }

        [TestMethod]
        public void ReplaceCustom()
        {
            // Use Regex.Replace method and custom function 
            // To find and replace
            // Reformat date YYYYMMDD => MMM-DD-YYYY
        }


        [TestMethod]
        public void SplitExample()
        {
            // Use Regex.Split method to split text based on regex pattern
        }
    }
}
