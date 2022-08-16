namespace Bank.Domain
{
    public class BankAccountSummary
    {
        public int Id { get; }
        public int HolderId { get; }
        public string HolderName { get; }
        public decimal Balance { get; }
        public decimal MinBalance { get; }

        public BankAccountSummary(int id, int holderId, string holderName, decimal balance, decimal minBalance)
        {
            Id = id;
            HolderId = holderId;
            HolderName = holderName;
            Balance = balance;
            MinBalance = minBalance;
        }
    }
}