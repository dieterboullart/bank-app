namespace Bank.Interfaces
{
    public interface ILogger
    {
        public void LogSuccess(string message);
        public void LogFailure(string message);
    }
}