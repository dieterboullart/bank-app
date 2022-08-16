using System;

namespace Bank.Domain
{
    public class BankAccountDepositTransaction : BankAccountTransaction
    {
        public BankAccountDepositTransaction(decimal amount, DateTime dateTime) : base(amount, dateTime)
        {
        }
    }
}