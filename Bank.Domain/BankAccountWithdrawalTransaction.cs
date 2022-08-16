using System;

namespace Bank.Domain
{
    public class BankAccountWithdrawalTransaction : BankAccountTransaction
    {
        public BankAccountWithdrawalTransaction(decimal amount, DateTime dateTime) : base(amount, dateTime)
        {
        }
    }
}