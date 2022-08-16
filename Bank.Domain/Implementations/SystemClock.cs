using System;
using Bank.Domain.Interfaces;
using Bank.Interfaces;

namespace Bank.Domain.Implementations
{
    public class SystemClock : IClock
    {
        public DateTime CurrentUtcTime => DateTime.UtcNow; 
    }
}