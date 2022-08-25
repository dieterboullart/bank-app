using System;

namespace Bank.Domain.Models
{
    public class BankAccountDepositTransaction : BankAccountTransaction
    {
        public BankAccountDepositTransaction(int? id, decimal amount, DateTime dateTime) : base(id, amount, dateTime)
        {
        }
    }
}