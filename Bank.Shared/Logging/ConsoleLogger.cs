using Bank.Shared.Logging.Interfaces;

namespace Bank.Shared.Logging
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