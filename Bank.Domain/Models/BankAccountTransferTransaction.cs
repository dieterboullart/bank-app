using System;

namespace Bank.Domain.Models
{
    public class BankAccountTransferTransaction : BankAccountTransaction
    {
        public int SourceBankAccountId { get; set; }
        public int DestinationBankAccountId { get; }

        public BankAccountTransferTransaction(int sourceBankAccountId, int destinationBankAccountId, decimal amount, DateTime dateTime) : base(amount, dateTime)
        {
            SourceBankAccountId = sourceBankAccountId;
            DestinationBankAccountId = destinationBankAccountId;
        }
    }
}