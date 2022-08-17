using System;

namespace Bank.Domain.Models
{
    public class BankAccountDepositTransaction : BankAccountTransaction
    {
        public BankAccountDepositTransaction(decimal amount, DateTime dateTime) : base(amount, dateTime)
        {
        }
    }
}