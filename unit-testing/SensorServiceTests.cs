using Xunit;
using TempSensorApp.services; 
using TempSensorModels;      

namespace unit_testing         
{

    public interface Iconsole
    {
        bool KeyAvailable {get;}
        ConsoleKeyInfo ReadKey(bool intercept);
        void WriteLine(string message);
    }
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
            // Arrange
            var service = new SensorService();

            // Act
            var sensor = service.InitSensor("TestSensor", "TestLocation", 22, 24);

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
                // Arrange
                var sensor = service.InitSensor("Sensor 1", "Data Center", 22, 24);

                // Act
                Console.WriteLine("Starting temperature sensor simulation...");

                // Assert
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
        [Fact]
        public void InitSensor_ShouldThrowException_WhenMinValueIsNegative() 
        {
            //Arrange
            var service = new SensorService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.InitSensor("TestSensor", "TestLocation", -1, 24));
        }

        [Fact]
        public void ValidateData_ShouldReturnFalse_WhenDataIsNaN()
        {
            // Arrange
            var service = new SensorService();
            var sensor = service.InitSensor("TestSensor", "TestLocation", 22, 24);

            // Act
            var isValid = service.ValidateData(double.NaN, sensor);

            // Assert
            Assert.False(isValid);
        }
        [Fact]
        public void StoreData_ShouldAddMultipleDataPointsToHistory()
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
            service.StoreData(sensor, 23.5);

            // Assert
            Assert.Equal(2, sensor.DataHistory.Count);
            Assert.Equal(22.5, sensor.DataHistory[0]);
            Assert.Equal(23.5, sensor.DataHistory[1]);
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnFalse_ForConsistentData()
        {
            // Arrange
            var service = new SensorService();
            var sensor = new Sensor
            {
                Name = "Test Sensor",
                DataHistory = new List<double> { 22, 22, 22, 22, 22 }
            };

            // Act
            var isAnomaly = service.AnomalyDetection(sensor);

            // Assert
            Assert.False(isAnomaly);
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnTrue_ForLargePositiveDeviation()
        {
            // Arrange
            var service = new SensorService();
            var sensor = new Sensor
            {
                Name = "Test Sensor",
                DataHistory = new List<double> { 22, 22, 22, 22, 29 }
            };

            // Act
            var isAnomaly = service.AnomalyDetection(sensor);

            // Assert
            Assert.True(isAnomaly);
        }

        [Fact]
        public void LogData_ShouldCreateNonEmptyLogFile()
        {
            // Arrange
            var service = new SensorService();
            var data = 22.5;

            // Act
            service.LogData(data);

            // Assert
            var logFileContent = File.ReadAllText("logs/sensor_log.txt");
            Assert.NotEmpty(logFileContent);
        }

        [Fact]
        public void InitializeAndStartSensor_ShouldThrowException_ForNullSensor()
        {
            // Arrange
            var helper = new ProgramHelper();
            SensorService service = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => helper.InitializeAndStartSensor(service));
        }

        [Fact]
        public void SimulateData_ShouldIncludeNoise()
        {
            // Arrange
            var service = new SensorService();
            var sensor = service.InitSensor("TestSensor", "TestLocation", 22, 24);

            // Act
            var data = service.SimulateData(sensor);

            // Assert
            Assert.True(data >= sensor.MinValue - 1 && data <= sensor.MaxValue + 1); // Includes noise range
        }

       [Fact]
        public void SimulateData_ShouldReturnOutOfRange_ForFaultySensor()
        {
            // Arrange
            var service = new SensorService();
            var sensor = new Sensor
            {
                Name = "Faulty Sensor",
                MinValue = 22,
                MaxValue = 24,
                IsFaulty = true
            };

            // Act
            var data = service.SimulateData(sensor);

            // Assert
            Assert.True(data > sensor.MaxValue || data < sensor.MinValue, "Expected data to be out of range for a faulty sensor.");
        }
        
        [Fact]
        public void AnomalyDetection_ShouldReturnFalse_ForLessThanFiveDataPoints()
        {
            // Arrange
            var service = new SensorService();
            var sensor = new Sensor
            {
                Name = "Test Sensor",
                DataHistory = new List<double> { 22, 23 }
            };

            // Act
            var isAnomaly = service.AnomalyDetection(sensor);

            // Assert
            Assert.False(isAnomaly);
        }
    }
}
