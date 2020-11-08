using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;

// Different namespace for hands-on-labs to prevent class naming conflict
namespace TestProjectShell
{
    /// <summary>
    /// Implement Sensor Data Parsing Code
    /// </summary>
    
    // TODO: Define SensorData metrics class 
    //   store data for one day

    [TestClass]
    public class SensorDataSolution
    {
        // TODO: Update path to point to your directory
        string directory = @"C:\Git\RegexDotNet\Data\SensorData";        
        
        
        public void ParseSensorData(string fileName)
        {
            // TODO: Implement parsing logic
            return;
        }

        [TestMethod]
        public void ProcessFile()
        {
            string fileName = directory + @"\" + "sensordata_small.txt";

            ParseSensorData(fileName);
        }
    }
}
