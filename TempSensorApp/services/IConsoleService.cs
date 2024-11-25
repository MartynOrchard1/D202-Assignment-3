namespace TempSensorApp.services
{
    public interface IConsoleService
    {
        bool KeyAvailable { get; }
        ConsoleKeyInfo ReadKey(bool intercept);
        void WriteLine(string message);
    }
}
