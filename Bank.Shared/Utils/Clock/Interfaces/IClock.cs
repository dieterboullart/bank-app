namespace Bank.Shared.Utils.Clock.Interfaces
{
    public interface IClock
    {
        DateTime CurrentUtcTime { get; }
    }
}