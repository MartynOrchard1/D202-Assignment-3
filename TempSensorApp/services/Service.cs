// Import Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TempSensorModels;

namespace TempSensorApp.services
{
    public class SensorService
    {
        private readonly IConsoleService _console;
        private readonly Random _random = new Random();

        // Constructor with dependency injection
        public SensorService(IConsoleService console)
        {
            _console = console;
        }

        // Initialize Sensor
        public virtual Sensor InitSensor(string name, string location, double minValue, double maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentException("MinValue must be less than MaxValue.");
            }

            if (minValue < 0)
            {
                throw new ArgumentException("MinValue can't be a negative number.");
            }

            return new Sensor
            {
                Name = name,
                Location = location,
                MinValue = minValue,
                MaxValue = maxValue,
            };
        }

        // Handle User Input
        public bool HandleUserInput()
        {
            if (_console.KeyAvailable)
            {
                var key = _console.ReadKey(true).Key;

                if (key == ConsoleKey.P)
                {
                    _console.WriteLine("\n--- MENU ---");
                    _console.WriteLine("1. Continue");
                    _console.WriteLine("2. Exit");
                    _console.WriteLine("Enter your choice: ");

                    var choice = _console.ReadKey(true).Key;

                    if (choice == ConsoleKey.D1 || choice == ConsoleKey.NumPad1)
                    {
                        _console.WriteLine("\nResuming Sensor Generation...");
                        return false; // Continue
                    }
                    else if (choice == ConsoleKey.D2 || choice == ConsoleKey.NumPad2)
                    {
                        _console.WriteLine("\nExiting Application...");
                        return true; // Exit
                    }
                    else
                    {
                        _console.WriteLine("\nInvalid Choice. Resuming sensor generation...");
                        return false; // Continue
                    }
                }
            }
            return false;
        }

        // Start the Sensor
        public virtual async Task StartSensor(Sensor sensor)
        {
            _console.WriteLine("Sensor Started. Press 'P' to pause and view options.");

            while (true)
            {
                if (HandleUserInput())
                {
                    break; // Exit the loop based on user input
                }

                var simulatedData = SimulateData(sensor);

                // Validate Data
                bool isValid = ValidateData(simulatedData, sensor);
                if (isValid)
                {
                    LogData(simulatedData);
                    StoreData(sensor, simulatedData);

                    if (AnomalyDetection(sensor))
                    {
                        _console.WriteLine("Anomaly detected!");
                    }
                }
                else
                {
                    _console.WriteLine($"Data is invalid: {simulatedData}");
                }

                await Task.Delay(1000); // Interval for Sensor Generation
            }
        }

        // Simulate Sensor Data
        public virtual double SimulateData(Sensor sensor)
        {
            double noise = _random.NextDouble() * 2 - 1; // Noise between -1 and 1
            double value = sensor.MinValue + noise;

            if (sensor.IsFaulty)
            {
                value += _random.NextDouble() * 5; // Simulates a fault
            }
            return Math.Round(value, 2);
        }

        // Validate Sensor Data
        public bool ValidateData(double data, Sensor sensor)
        {
            return data >= sensor.MinValue && data <= sensor.MaxValue;
        }

        // Log Sensor Data
        public virtual void LogData(double data)
        {
            string log = $"{DateTime.Now}: {data}"; // Logs timestamp with data
            _console.WriteLine(log); // Output log to the console
            File.AppendAllText("logs/sensor_log.txt", log + Environment.NewLine); // Append log to file
        }

        // Store Sensor Data
        public void StoreData(Sensor sensor, double data)
        {
            sensor.DataHistory.Add(data);
            if (sensor.DataHistory.Count > 100)
            {
                sensor.DataHistory.RemoveAt(0); // Limit data store to 100 entries
            }
        }

        // Detect Anomalies
        public virtual bool AnomalyDetection(Sensor sensor)
        {
            if (sensor.DataHistory.Count < 5) return false;

            double average = sensor.DataHistory.Average();
            double lastReading = sensor.DataHistory.Last();
            return Math.Abs(lastReading - average) > 2; // Detect anomaly if deviation > 2
        }
    }
}
