using System.Collections.Generic;
using System.Linq;
using Bank.Interfaces;
using Bank.Models;

namespace Bank.Introduction.Tests.Fakes
{
    public class FakeBankRepository : IBankRepository
    {
        private IList<BankAccount> _bankAccountsForGet;
        private readonly Dictionary<int, BankAccount> _bankAccountsForGetAll = new();
        private string _saveBankExceptionMessage;

        public List<BankAccount> SavedBankAccounts { get; } = new();
        
        public void SetupGetAll(IList<BankAccount> bankAccounts)
        {
            _bankAccountsForGet = bankAccounts;
        }
        
        public IList<BankAccount> GetAll()
        {
            return _bankAccountsForGet?.ToList();
        }

        public void SetupGet(int accountId, BankAccount bankAccount)
        {
            _bankAccountsForGetAll[accountId] = bankAccount;
        }
        public BankAccount Get(int accountId)
        {
            return _bankAccountsForGetAll.TryGetValue(accountId, out var bankAccount) ? bankAccount : null;
        }

        public void SetupSaveThrowsBankException(string message)
        {
            _saveBankExceptionMessage = message;
        }
        
        public void Save(BankAccount bankAccount)
        {
            SavedBankAccounts.Add(bankAccount);
            
            if (_saveBankExceptionMessage != null)
            {
                throw new BankException(_saveBankExceptionMessage);
            }
        }
    }
}