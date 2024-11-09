// Import Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TempSensorModels;

// Setup namespace
namespace TempSensorApp.services 
{
    // Define Class
    public class SensorService 
    {
        // Random init
        private Random _random = new Random(); 
        // Setup Sensor
        public Sensor InitSensor(string name, string location, double minValue, double maxValue) 
        {
            return new Sensor { Name = name, Location = location, MinValue = minValue, MaxValue = maxValue }; 
        }

        // Start the Sensor
        public async Task StartSensor(Sensor sensor)
        {
            while (true)
            {
                var simulatedData = SimulateData(sensor); 
                bool isValid = ValidateData(simulatedData, sensor);

                if (isValid)
                {
                    LogData(simulatedData); 
                    StoreData(sensor, simulatedData); 

                    if (AnomalyDetection(sensor)) 
                    {
                        Console.WriteLine("Anomaly detected!"); 
                    }
                }

                await Task.Delay(1000); // Delay Task 
            }
        }

        // Simulate Sensor Data
        public double SimulateData(Sensor sensor)
        {
            double noise = _random.NextDouble() * 2 - 1; // Looks for noise between -1 and 1
            double value = sensor.MinValue + noise;
            if (sensor.IsFaulty) value += _random.NextDouble() * 5; // Fault Simulation 
            return value;
        }

        // Validate Sensor Data
        public bool ValidateData(double data, Sensor sensor)
        {
            return data >= sensor.MinValue && data <= sensor.MaxValue; // Validates the Sensor Data 
        }

        // Log Sensor Data
        public void LogData(double data) 
        {
            string log = $"{DateTime.Now}: {data}"; // Logs the date alongside the sensor data
            Console.WriteLine(log); // Log the string to the console 
            File.AppendAllText("logs/sensor_log.txt", log + Environment.NewLine); // Logs out into the logs folder
        }

        // Store Sensor Data
        public void StoreData(Sensor sensor, double data) 
        {
            sensor.DataHistory.Add(data);
            if (sensor.DataHistory.Count > 100)
            {
                sensor.DataHistory.RemoveAt(0); // Limits Data Store to 100 entries
            }
        }

        // Detect Anomalies
        public bool AnomalyDetection(Sensor sensor) 
        {
            if (sensor.DataHistory.Count < 5) return false;
            double average = sensor.DataHistory.Average();
            double lastReading = sensor.DataHistory.Last();
            return Math.Abs(lastReading - average) > 2; // There's an anomaly if deviation is greater than 2 // [ISSUE #20] Corrected `Math.abs` to `Math.Abs`
        }
    }
}
