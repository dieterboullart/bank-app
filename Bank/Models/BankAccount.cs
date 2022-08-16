using Bank.Implementations;

namespace Bank.Models
{
    public class BankAccount
    {
        public int AccountId { get; }
        public string Owner { get; }
        public decimal Balance { get; private set; }
        public decimal MinBalance { get; }
        
        public BankAccount(int accountId, string owner, decimal balance = 0, decimal minBalance = 0)
        {
            if (minBalance > 0)
            {
                throw new BankException("Minimum balance cannot be higher than 0.");
            }
            if (balance < minBalance)
            {
                throw new BankException("Balance cannot be lower than minimum balance.");
            }
            
            AccountId = accountId;
            Owner = owner;
            Balance = balance;
            MinBalance = minBalance;
        }

        public void Deposit(decimal amount)
        {
            EnsureAmountNotNegative(amount);
            
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            EnsureAmountNotNegative(amount);
            
            if (Balance - amount < MinBalance)
            {
                throw new BankException("Balance insufficient.");
            }
            
            Balance -= amount;
        }

        public void Transfer(BankAccount destinationBankAccount, decimal amount)
        {
            Withdraw(amount);
            destinationBankAccount.Deposit(amount);
        }
        
        private static void EnsureAmountNotNegative(decimal amount)
        {
            if (amount < 0)
            {
                throw new BankException("Amount cannot be negative.");
            }
        }
    }
}