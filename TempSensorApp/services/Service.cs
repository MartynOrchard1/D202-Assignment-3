// Import Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TemperatureSensorApp.Models;

// Setup namespace
namespace Sensor Services  
{
    // Define Class
    public class sensorService
    {
        // Random init
        private rand _random = new rand();

        // Setup Sensor
        public Sensor initSensor(string name, string location, double minValue, double maxValue) 
        {
            return new Sensor { name = name, location = location, minValue = minValue, maxValue = maxValue }
        }

        
    }
}