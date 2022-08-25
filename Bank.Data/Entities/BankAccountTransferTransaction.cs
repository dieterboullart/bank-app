namespace Bank.Data.Entities
{
    public class BankAccountTransferTransaction : BankAccountTransaction
    {
        public int SourceBankAccountId { get; set; }
        public int DestinationBankAccountId { get; set; }
        
        #region Navigation properties

        public BankAccount SourceBankAccount { get; set; }
        
        public BankAccount DestinationBankAccount { get; set; }

        #endregion
    }
}