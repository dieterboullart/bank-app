using System;

namespace Bank.Domain.Models
{
    public class BankAccountTransferTransaction : BankAccountTransaction
    {
        public int SourceBankAccountId { get; }
        public int DestinationBankAccountId { get; }

        public BankAccountTransferTransaction(int? id, int sourceBankAccountId, int destinationBankAccountId, decimal amount, DateTime dateTime) : base(id, amount, dateTime)
        {
            SourceBankAccountId = sourceBankAccountId;
            DestinationBankAccountId = destinationBankAccountId;
        }
    }
}