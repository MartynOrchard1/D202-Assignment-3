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

        [Fact]
        public void LogData_ShouldWriteToLogFile()  // Log Data From Service.cs
        {
            // Arrange
            var service = new SensorService();
            var data = 22.5;

            // Act
            service.LogData(data);

            // Assert
            var logFileContent = File.ReadAllText("logs/sensor_log.txt");
            Assert.Contains(data.ToString(), logFileContent);
        }


        public class ProgramHelper
        {
            public Sensor InitializeAndStartSensor(SensorService service)
            {
                var sensor = service.InitSensor("Sensor 1", "Data Center", 22, 24);
                Console.WriteLine("Starting temperature sensor simulation...");
                return sensor;
            }
        }

        [Fact]
        public void InitializeAndStartSensor_ShouldReturnValidSensor()
        {
            // Arrange
            var service = new SensorService();
            var helper = new ProgramHelper();

            // Act
            var sensor = helper.InitializeAndStartSensor(service);

            // Assert
            Assert.NotNull(sensor);
            Assert.Equal("Sensor 1", sensor.Name);
            Assert.Equal("Data Center", sensor.Location);
        }

        [Fact]
        public void StoreData_ShouldAddDataToHistory()
        {
            // Arrange
            var service = new SensorService();
            var sensor = new Sensor
            {
                Name = "Test Sensor",
                DataHistory = new List<double>()
            };

            // Act
            service.StoreData(sensor, 22.5);

            // Assert
            Assert.Single(sensor.DataHistory);
            Assert.Equal(22.5, sensor.DataHistory[0]);
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnTrue_ForSignificantDeviation()
        {
            // Arrange
            var service = new SensorService();
            var sensor = new Sensor
            {
                Name = "Test Sensor",
                DataHistory = new List<double> { 22, 22, 22, 22, 28 }
            };

            // Act
            var isAnomaly = service.AnomalyDetection(sensor);

            // Assert
            Assert.True(isAnomaly);
        }

        [Fact]
        public void InitSensor_ShouldThrowException_ForInvalidRange()
        {
            // Arrange
            var service = new SensorService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.InitSensor("Sensor 1", "Lab", 25, 22));
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnFalse_ForEmptyHistory()
        {
            // Arrange
            var service = new SensorService();
            var sensor = new Sensor { DataHistory = new List<double>() };

            // Act
            var isAnomaly = service.AnomalyDetection(sensor);

            // Assert
            Assert.False(isAnomaly);
        }

        
    }
}
