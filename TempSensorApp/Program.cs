// Import Statements
using TempSensorApp.services;
using TempSensorApp.models;
using System;
using System.Threading.tasks;

namespace TempSensorApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sensorService = new SensorServeice();
            var sensor = sensorService.initSensor("Sensor 1", "Data Center", 22, 24);

            console.writeline("Starting temperature sensor simulation...");
            await sensorService.StartSensor(sensor);
        }
    }

}