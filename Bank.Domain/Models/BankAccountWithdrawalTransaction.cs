using System;

namespace Bank.Domain.Models
{
    public class BankAccountWithdrawalTransaction : BankAccountTransaction
    {
        public BankAccountWithdrawalTransaction(decimal amount, DateTime dateTime) : base(amount, dateTime)
        {
        }
    }
}