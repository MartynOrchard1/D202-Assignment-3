using System;

public interface IConsole
{
    bool KeyAvailable { get; }
    ConsoleKeyInfo ReadKey(bool intercept);
    void WriteLine(string message);
}
