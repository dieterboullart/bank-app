using System;

namespace Bank.Domain.Models
{
    public class BankAccountWithdrawalTransaction : BankAccountTransaction
    {
        public BankAccountWithdrawalTransaction(int? id, decimal amount, DateTime dateTime) : base(id, amount, dateTime)
        {
        }
    }
}