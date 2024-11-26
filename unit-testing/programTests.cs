using Xunit;
using Moq;
using System.IO;
using System.Threading.Tasks;
using TempSensorApp.services;
using TempSensorModels;

namespace unit_testing
{
    public class ProgramTests
    {
        [Fact]
        public async Task RunAsync_ShouldInitializeAndRunSensor()
        {
            // Arrange
            var mockConsoleService = new Mock<IConsoleService>();
            var mockSensorService = new Mock<SensorService>(mockConsoleService.Object) { CallBase = true };
            var sensor = new Sensor
            {
                Name = "Sensor 1",
                Location = "Data Center",
                MinValue = 22,
                MaxValue = 24
            };

            mockSensorService.Setup(s => s.InitSensor("Sensor 1", "Data Center", 22, 24)).Returns(sensor);
            mockSensorService.Setup(s => s.StartSensor(sensor)).Returns(Task.CompletedTask);

            // Act
            await Program.RunAsync();

            // Assert
            mockSensorService.Verify(s => s.InitSensor("Sensor 1", "Data Center", 22, 24), Times.Once);
            mockSensorService.Verify(s => s.StartSensor(sensor), Times.Once);
        }

        [Fact]
        public async Task RunAsync_ShouldWriteStartingMessageToConsole()
        {
            // Arrange
            var output = new StringWriter();
            Console.SetOut(output);

            var mockConsoleService = new Mock<IConsoleService>();
            var mockSensorService = new Mock<SensorService>(mockConsoleService.Object) { CallBase = true };

            var sensor = new Sensor
            {
                Name = "Sensor 1",
                Location = "Data Center",
                MinValue = 22,
                MaxValue = 24
            };

            mockSensorService.Setup(s => s.InitSensor("Sensor 1", "Data Center", 22, 24)).Returns(sensor);
            mockSensorService.Setup(s => s.StartSensor(sensor)).Returns(Task.CompletedTask);

            // Act
            await Program.RunAsync();

            // Assert
            Assert.Contains("Starting temperature sensor simulation...", output.ToString());
        }
    }
}
