using Xunit;
using Moq;
using TempSensorApp.services;
using TempSensorModels;
using System;
using System.IO;
using System.Collections.Generic;

namespace unit_testing
{
    public class SensorServiceTests
    {
        private readonly Mock<IConsoleService> _mockConsole;
        private readonly SensorService _service;

        public SensorServiceTests()
        {
            _mockConsole = new Mock<IConsoleService>();
            _service = new SensorService(_mockConsole.Object);
        }

        [Fact]
        public void HandleUserInput_NoKeyAvailable_ReturnsFalse()
        {
            // Arrange
            _mockConsole.Setup(c => c.KeyAvailable).Returns(false);

            // Act
            var result = _service.HandleUserInput();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HandleUserInput_KeyNotP_ReturnsFalse()
        {
            // Arrange
            _mockConsole.Setup(c => c.KeyAvailable).Returns(true);
            _mockConsole.Setup(c => c.ReadKey(true)).Returns(new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false));

            // Act
            var result = _service.HandleUserInput();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HandleUserInput_KeyP_Choice1_ReturnsFalse()
        {
            // Arrange
            _mockConsole.SetupSequence(c => c.ReadKey(true))
                .Returns(new ConsoleKeyInfo('P', ConsoleKey.P, false, false, false)) // Key `P`
                .Returns(new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false)); // Choice `1`
            _mockConsole.Setup(c => c.KeyAvailable).Returns(true);

            // Act
            var result = _service.HandleUserInput();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HandleUserInput_KeyP_Choice2_ReturnsTrue()
        {
            // Arrange
            _mockConsole.SetupSequence(c => c.ReadKey(true))
                .Returns(new ConsoleKeyInfo('P', ConsoleKey.P, false, false, false)) // Key `P`
                .Returns(new ConsoleKeyInfo('2', ConsoleKey.D2, false, false, false)); // Choice `2`
            _mockConsole.Setup(c => c.KeyAvailable).Returns(true);

            // Act
            var result = _service.HandleUserInput();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HandleUserInput_KeyP_InvalidChoice_ReturnsFalse()
        {
            // Arrange
            _mockConsole.SetupSequence(c => c.ReadKey(true))
                .Returns(new ConsoleKeyInfo('P', ConsoleKey.P, false, false, false)) // Key `P`
                .Returns(new ConsoleKeyInfo('X', ConsoleKey.X, false, false, false)); // Invalid choice
            _mockConsole.Setup(c => c.KeyAvailable).Returns(true);

            // Act
            var result = _service.HandleUserInput();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HandleUserInput_LogsMenu_WhenKeyPIsPressed()
        {
            // Arrange
            _mockConsole.Setup(c => c.KeyAvailable).Returns(true);
            _mockConsole.Setup(c => c.ReadKey(true)).Returns(new ConsoleKeyInfo('P', ConsoleKey.P, false, false, false));

            // Act
            _service.HandleUserInput();

            // Assert
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains("MENU"))), Times.Once);
        }

        [Fact]
        public void InitSensor_ShouldReturnSensor_WithCorrectValues()
        {
            // Arrange
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);


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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
        public void WriteLine_ShouldOutputMessageToConsole()
        {
            // Arrange
            var consoleService = new ConsoleService();
            var output = new StringWriter();
            Console.SetOut(output);
            var message = "Test message";

            // Act
            consoleService.WriteLine(message);

            // Assert
            Assert.Contains(message, output.ToString());
        }

        [Fact]
        public void InitializeAndStartSensor_ShouldReturnValidSensor()
        {
            // Arrange
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);


            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.InitSensor("Sensor 1", "Lab", 25, 22));
        }

        [Fact]
        public void AnomalyDetection_ShouldReturnFalse_ForEmptyHistory()
        {
            // Arrange
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);


            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.InitSensor("TestSensor", "TestLocation", -1, 24));
        }

        [Fact]
        public void ValidateData_ShouldReturnFalse_WhenDataIsNaN()
        {
            // Arrange
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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
            var mockConsole = new Mock<IConsoleService>();
            var service = new SensorService(mockConsole.Object);

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

        [Fact]
        public async Task StartSensor_ShouldOutputSensorStartedMessage()
        {
            // Arrange
            var mockConsole = new Mock<IConsoleService>();
            var mockService = new SensorService(mockConsole.Object);
            var sensor = mockService.InitSensor("Sensor 1", "Data Center", 22, 24);

            mockConsole.Setup(c => c.KeyAvailable).Returns(false); // No key pressed initially

            // Act
            var task = mockService.StartSensor(sensor);

            // Allow the method to run briefly
            await Task.Delay(200);

            // Assert
            mockConsole.Verify(c => c.WriteLine("Sensor Started. Press 'P' to pause and view options."), Times.Once);
        }

        [Fact]
        public async Task StartSensor_ShouldCallSimulateData()
        {
            // Arrange
            var mockConsole = new Mock<IConsoleService>();
            var mockService = new Mock<SensorService>(mockConsole.Object) { CallBase = true }; // Enable partial mocking
            var sensor = mockService.Object.InitSensor("Sensor 1", "Data Center", 22, 24);

            mockConsole.Setup(c => c.KeyAvailable).Returns(false); // No key press initially
            mockConsole.SetupSequence(c => c.KeyAvailable)
                .Returns(false)  // Run the loop
                .Returns(true);  // Exit condition
            mockConsole.Setup(c => c.ReadKey(true))
                .Returns(new ConsoleKeyInfo('P', ConsoleKey.P, false, false, false)); // Exit condition

            // Act
            var task = mockService.Object.StartSensor(sensor);
            await Task.Delay(200); // Let the loop run briefly

            // Assert
            mockService.Verify(s => s.SimulateData(sensor), Times.AtLeastOnce);
        }


    }
}
