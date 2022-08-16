using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Domain
{
    public class BankAccount
    {
        private readonly List<BankAccountTransaction> _transactions;
        
        public int Id { get; set; }
        public Person Holder { get; }
        public decimal Balance { get; private set; }
        public decimal MinBalance { get; }
        public IReadOnlyCollection<BankAccountTransaction> Transactions => _transactions.ToList();

        public BankAccount(int id, Person holder, decimal balance, decimal minBalance, IEnumerable<BankAccountTransaction> transactions)
                :this(holder, balance, minBalance)
        {
            Id = id;
            if (transactions == null) return;
            _transactions = transactions.ToList();
        }
        
        public BankAccount(Person holder, decimal balance = 0, decimal minBalance = 0)
        {
            if (minBalance > 0)
            {
                throw new BankException("Minimum balance cannot be higher than 0.");
            }
            if (balance < minBalance)
            {
                throw new BankException("Balance cannot be lower than minimum balance.");
            }
            
            Holder = holder;
            Balance = balance;
            MinBalance = minBalance;
            _transactions = new List<BankAccountTransaction>();
        }

        public IList<BankAccountTransaction> GetTransactions()
        {
            return Transactions.ToList();
        }
        
        public void Deposit(decimal amount, DateTime dateTime)
        {
            EnsureAmountNotNegative(amount);
            
            Balance += amount;
            _transactions.Add(new BankAccountDepositTransaction(amount, dateTime));
        }

        public void Withdraw(decimal amount, DateTime dateTime)
        {
            EnsureAmountNotNegative(amount);
            EnsureAmountCanBeWithdrawn(amount);
            
            Balance -= amount;
            _transactions.Add(new BankAccountWithdrawalTransaction(amount, dateTime));
        }
        
        public void Transfer(BankAccount destinationBankAccount, decimal amount, DateTime dateTime)
        {
            EnsureAmountNotNegative(amount);
            EnsureAmountNotNegative(amount);
            
            if (Balance - amount < MinBalance)
            {
                throw new BankException("Balance insufficient.");
            }
            
            Balance -= amount;
            
            destinationBankAccount.Balance += amount;

            var transaction = new BankAccountTransferTransaction(Id, destinationBankAccount.Id, amount, dateTime);
            
            _transactions.Add(transaction);
            destinationBankAccount._transactions.Add(transaction);
        }
        
        private static void EnsureAmountNotNegative(decimal amount)
        {
            if (amount < 0)
            {
                throw new BankException("Amount cannot be negative.");
            }
        }
        
        private void EnsureAmountCanBeWithdrawn(decimal amount)
        {
            if (Balance - amount < MinBalance)
            {
                throw new BankException("Balance insufficient.");
            }
        }
    }
}