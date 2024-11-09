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
            var sensorService = new SensorService();
            var sensor = sensorService.initSensor("Sensor 1", "Data Center", 22, 24);

            Console.WriteLine("Starting temperature sensor simulation...");
            await sensorService.StartSensor(sensor);
        }
    }

}