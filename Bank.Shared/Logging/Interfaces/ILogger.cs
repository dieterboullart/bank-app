namespace Bank.Shared.Logging.Interfaces
{
    public interface ILogger
    {
        public void LogSuccess(string message);
        public void LogFailure(string message);
    }
}