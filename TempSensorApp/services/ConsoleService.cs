namespace TempSensorApp.services
{
    public class ConsoleService : IConsoleService
    {
        public bool KeyAvailable => Console.KeyAvailable;

        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

        public void WriteLine(string message) => Console.WriteLine(message);
    }
}
