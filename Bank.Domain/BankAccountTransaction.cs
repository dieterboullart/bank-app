using System;

namespace Bank.Domain
{
    public abstract class BankAccountTransaction
    {
        public DateTime DateTime { get; }
        public decimal Amount { get; }

        protected BankAccountTransaction(decimal amount, DateTime dateTime)
        {
            DateTime = dateTime;
            Amount = amount;
        }
    }
}