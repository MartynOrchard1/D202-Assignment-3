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
            if (minValue >= maxValue) 
            {
                throw new ArgumentException("MinValue must be less than Maxvalue");
            }

            if (minValue < 0) 
            {
                throw new ArgumentException("MinValue Can't be a negative Number");
            }

            return new Sensor 
            { 
                Name = name, 
                Location = location, 
                MinValue = minValue, 
                MaxValue = maxValue, 

            }; 
        }

        // Start the Sensor
        public async Task StartSensor(Sensor sensor)
        {
            Console.WriteLine("Press any key to stop the sensor...");

            while (true)
            {
                // Check if user presses a key on their keyboard...
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.P) 
                    {
                        // Pause and Display user Options
                        Console.WriteLine("\n--- MENU ---");
                        Console.WriteLine("1. Continue");
                        Console.WriteLine("2. Exit");
                        Console.WriteLine("Enter your choice: ");

                        var choice = Console.ReadKey(true).Key;
                        if (choice == ConsoleKey.D1 || choice == ConsoleKey.NumPad1)
                        {
                            Console.WriteLine("\nResuming Sensor Generation");
                            continue;
                        }
                        else if (choice == ConsoleKey.D2 || choice == ConsoleKey.NumPad2) 
                        {
                            Console.WriteLine("\nExiting Application...");
                            break;
                        }
                        else {
                            Console.WriteLine("\nInvalid Choice. Resuming sensor generation...");
                            continue;
                        }
                    }
                }

                var simulatedData = SimulateData(sensor); 

                // Validate
                bool isValid = ValidateData(simulatedData, sensor);
                if (isValid)
                {
                    // Log The Data
                    LogData(simulatedData); 
                    StoreData(sensor, simulatedData); 

                    if (AnomalyDetection(sensor)) 
                    {
                        Console.WriteLine("Anomaly detected!"); // If there's an anomaly do this
                    }
                }
                else 
                {   
                    Console.WriteLine($"Data Is Invalid {simulatedData}");
                }

                await Task.Delay(1000); // Interval for Sensor Generation
            }
        }

        // Simulate Sensor Data
        public double SimulateData(Sensor sensor)
        {
            double noise = _random.NextDouble() * 2 - 1; // Looks for noise between -1 and 1
            double value = sensor.MinValue + noise;
            
            if (sensor.IsFaulty) 
            {
                value += _random.NextDouble() * 5; // Simulates a Fault
            }
            return Math.Round(value, 2);
        }

        // Validate Sensor Data
        public bool ValidateData(double data, Sensor sensor)
        {
            return data >= sensor.MinValue && data <= sensor.MaxValue;
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
