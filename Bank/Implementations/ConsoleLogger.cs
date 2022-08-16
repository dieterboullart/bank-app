using System;
using Bank.Interfaces;

namespace Bank.Implementations
{
    public class ConsoleLogger : ILogger
    {
        public void LogSuccess(string message)
        {
            Console.WriteLine($"SUCCESS: {message}");
        }

        public void LogFailure(string message)
        {
            Console.WriteLine($"FAILURE: {message}");
        }
    }
}