using Bank.Interfaces;

namespace Bank.Introduction.Tests.Fakes
{
    public class FakeLogger : ILogger
    {
        public string SuccessMessage { get; private set; }
        public string FailureMessage { get; private set; }
        
        public void LogSuccess(string message)
        {
            SuccessMessage = message;
        }

        public void LogFailure(string message)
        {
            FailureMessage = message;
        }
    }
}