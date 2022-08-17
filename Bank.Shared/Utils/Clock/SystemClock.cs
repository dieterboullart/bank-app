using Bank.Shared.Utils.Clock.Interfaces;

namespace Bank.Shared.Utils.Clock
{
    public class SystemClock : IClock
    {
        public DateTime CurrentUtcTime => DateTime.UtcNow; 
    }
}