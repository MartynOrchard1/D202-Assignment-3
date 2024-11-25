using System;
using System.Collections.Generic;

public class MockConsole : IConsole
{
    private readonly Queue<ConsoleKeyInfo> _keys;
    public List<string> Output { get; } = new List<string>();

    public MockConsole(IEnumerable<ConsoleKeyInfo> keys)
    {
        _keys = new Queue<ConsoleKeyInfo>(keys);
    }

    public bool KeyAvailable => _keys.Count > 0;

    public ConsoleKeyInfo ReadKey(bool intercept) => _keys.Dequeue();

    public void WriteLine(string message) => Output.Add(message);
}
