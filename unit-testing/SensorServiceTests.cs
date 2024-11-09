using Xunit;
using TempSensorApp.services; 
using TempSensorModels;      

namespace unit_testing         
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
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            Assert.Equal("TestSensor", sensor.Name);
            Assert.Equal("TestLocation", sensor.Location);
            Assert.Equal(22, sensor.MinValue);
            Assert.Equal(24, sensor.MaxValue);
        }

        [Fact]
        public void SimulateData_ShouldReturnWithinRange()
        {
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            var data = _service.SimulateData(sensor);
            Assert.InRange(data, sensor.MinValue - 1, sensor.MaxValue + 1); // Account for noise
        }

        [Fact]
        public void ValidateData_ShouldReturnTrue_WhenDataIsWithinRange()
        {
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            var validData = 23;
            var isValid = _service.ValidateData(validData, sensor);
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateData_ShouldReturnFalse_WhenDataIsOutOfRange()
        {
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            var invalidData = 25;
            var isValid = _service.ValidateData(invalidData, sensor);
            Assert.False(isValid);
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnTrue_WhenDataIsOutOfAverageRange()
        {
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            sensor.DataHistory.AddRange(new double[] { 22, 22, 23, 23, 22 });
            var anomalyData = 28;
            sensor.DataHistory.Add(anomalyData);
            var isAnomaly = _service.AnomalyDetection(sensor);
            Assert.True(isAnomaly);
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnFalse_WhenDataIsWithinAverageRange()
        {
            var sensor = _service.InitSensor("TestSensor", "TestLocation", 22, 24);
            sensor.DataHistory.AddRange(new double[] { 22, 23, 23, 22, 22 });
            var isAnomaly = _service.AnomalyDetection(sensor);
            Assert.False(isAnomaly);
        }
    }
}
