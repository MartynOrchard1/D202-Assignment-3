using Xunit;
using TempSensorModels; // Matches the namespace in Sensor.cs
using TempSensorApp.services; // Matches the namespace in Service.cs
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
        public void InitSensor_ShouldReturnSensor_WithCorrectValues()
        {
            // Arrange & Act
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);

            // Assert
            Assert.Equal("TestSensor", sensor.Name);
            Assert.Equal("TestLocation", sensor.Location);
            Assert.Equal(22, sensor.MinValue);
            Assert.Equal(24, sensor.MaxValue);
        }

        [Fact]
        public void SimulateData_ShouldReturnWithinRange()
        {
            // Arrange
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);

            // Act
            var data = _service.SimulateData(sensor);

            // Assert
            Assert.InRange(data, sensor.MinValue - 1, sensor.MaxValue + 1); // Account for noise
        }

        [Fact]
        public void ValidateData_ShouldReturnTrue_WhenDataIsWithinRange()
        {
            // Arrange
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            var validData = 23;

            // Act
            var isValid = _service.ValidateData(validData, sensor);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateData_ShouldReturnFalse_WhenDataIsOutOfRange()
        {
            // Arrange
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            var invalidData = 25;

            // Act
            var isValid = _service.ValidateData(invalidData, sensor);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnTrue_WhenDataIsOutOfAverageRange()
        {
            // Arrange
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            sensor.DataHistory.AddRange(new double[] { 22, 22, 23, 23, 22 });
            var anomalyData = 28;

            // Act
            sensor.DataHistory.Add(anomalyData);
            var isAnomaly = _service.AnomalyDetection(sensor);

            // Assert
            Assert.True(isAnomaly);
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnFalse_WhenDataIsWithinAverageRange()
        {
            // Arrange
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            sensor.DataHistory.AddRange(new double[] { 22, 23, 23, 22, 22 });

            // Act
            var isAnomaly = _service.AnomalyDetection(sensor);

            // Assert
            Assert.False(isAnomaly);
        }
    }
}
