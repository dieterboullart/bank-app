namespace Bank.Data.Entities
{
    public class BankAccountTransferTransaction : Data.Entities.BankAccountTransaction
    {
        public int SourceBankAccountId { get; set; }
        public int DestinationBankAccountId { get; set; }
    }
}