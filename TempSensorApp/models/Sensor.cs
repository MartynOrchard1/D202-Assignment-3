// Import Statements
using System;
using System.Collections.Generic;

// Define Sensor Object
namespace TempSensorModels 
{
    public class Sensor 
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public List<double> DataHistory { get; set; } = new List<double>();
        public bool IsFaulty { get; set; }

    }
}