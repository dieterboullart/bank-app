namespace Bank.Data.Entities
{
    public class BankAccountTransferTransaction : BankAccountTransaction
    {
        public int SourceBankAccountId { get; set; }
        public int DestinationBankAccountId { get; set; }
    }
}