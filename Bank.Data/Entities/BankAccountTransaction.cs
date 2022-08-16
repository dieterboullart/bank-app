using System;

namespace Bank.Data.Entities
{
    public abstract class BankAccountTransaction
    {
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
    }
}