// Import Statements
using TempSensorApp.services;
using TempSensorModels;
using System;
using System.Threading.Tasks;

namespace TempSensorApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create an instance of ConsoleService (real implementation of IConsoleService)
            var consoleService = new ConsoleService();

            // Pass the ConsoleService instance to SensorService
            var sensorService = new SensorService(consoleService);

            // Initialize the sensor
            var sensor = sensorService.InitSensor("Sensor 1", "Data Center", 22, 24);

            Console.WriteLine("Starting temperature sensor simulation...");
            await sensorService.StartSensor(sensor);
        }
    }
}
