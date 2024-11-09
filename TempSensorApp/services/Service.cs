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

        // Start the Sensor
        public async Task StartSensor(Sensor sensor)
        {
            while (true) 
            {
                var simulatedData = simulateData(sensor);
                bool isValid = ValidateData(simulatedData, sensor);

                if (isValid)
                {
                    LogData(simulatedData);
                    StoreData(sensor, simulatedData);

                    if (Anomaly(sensor))
                    {
                        Console.writeline("Anomaly detected!");
                    }
                }
            }

            await Task.delay(1000); // Delay Task
        }

        // Simulate Sensor Data
        public double SimulateData(Sensor sensor)
        {
            double noise = _random.NextDouble() * 2 - 1; // Looks for noise between -1 and 1
            double value = sensor.MinValue + noise;
            if (sensor.isFaulty) value += _random.NextDouble * 5; // Fault Simulation
            return value;
        }

        // Validate Sensor Data
        public bool validateData(double data, Sensor sensor)
        {
            return data >=sensor.MinValue && data <= sensor.maxValue; // Validates the Sensor Data
        }

        // Log Sensor Data
        public void Log(double data) 
        {
            string log = $"{DateTime.Now}: {data}"; // Logs the date alongside the sensor data
            Console.writeline(log); // Log the string to the console
            File.AppendAllText("logs/sensor_log.txt", log + Environment.NewLine); // Logs out into the logs folder.
        }
    }
}