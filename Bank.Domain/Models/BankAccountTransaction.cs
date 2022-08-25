using System;

namespace Bank.Domain.Models
{
    public abstract class BankAccountTransaction
    {
        public int Id { get; }
        public DateTime DateTime { get; }
        public decimal Amount { get; }

        protected BankAccountTransaction(int? id, decimal amount, DateTime dateTime)
        {
            Id = id ?? 0;
            DateTime = dateTime;
            Amount = amount;
        }
    }
}