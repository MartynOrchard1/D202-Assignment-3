using Xunit;
using TemperatureSensorApp.Models;
using SensorServices;
using System;
using System.IO;
using System.Linq;

namespace TempSensorApp.Tests
{
    public class SensorServiceTests
    {
        private readonly SensorService _service;

        public SensorServiceTests()
        {
            _service = new SensorService();
        }

        [Fact]
        public void InitSensor()
        {
            // Arrange & Act
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);

            // Assert
            Assert.Equal("TestSensor", sensor.Name);
            Assert.Equal("TestLocation", sensor.Location);
            Assert.Equal(22, sensor.MinValue);
            Assert.Equal(24, sensor.MaxValue);
        }

    }
}
