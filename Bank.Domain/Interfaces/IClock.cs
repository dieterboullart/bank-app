using System;

namespace Bank.Domain.Interfaces
{
    public interface IClock
    {
        DateTime CurrentUtcTime { get; }
    }
}