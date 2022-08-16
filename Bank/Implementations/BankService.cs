using System.Collections.Generic;
using System.Linq;
using Bank.Interfaces;
using Bank.Models;

namespace Bank.Implementations
{
    
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly ILogger _logger;

        public BankService(IBankRepository bankRepository, ILogger logger)
        {
            _bankRepository = bankRepository;
            _logger = logger;
        }

        public IList<BankAccount> GetAllAccounts()
        {
            return _bankRepository.GetAll().OrderBy(x => x.AccountId).ToList();
        }

        public bool Deposit(int bankAccountId, decimal amount)
        {
            if (!TryGetBankAccount(bankAccountId, out var bankAccount))
            {
                return false;
            }
            
            try
            {
                bankAccount.Deposit(amount);
                _bankRepository.Save(bankAccount);
                
                _logger.LogSuccess($"Deposited {amount} EUR to account {bankAccountId}.");
                return true;
            }
            catch (BankException bankException)
            {
                _logger.LogFailure(bankException.Message);
                return false;
            }
        }
        
        public bool Withdraw(int bankAccountId, decimal amount)
        {
            if (!TryGetBankAccount(bankAccountId, out var bankAccount)) return false;
            
            try
            {
                bankAccount.Withdraw(amount);
                _bankRepository.Save(bankAccount);
                
                _logger.LogSuccess($"Withdrew {amount} EUR from account {bankAccountId}.");
                
                return true;
            }
            catch (BankException bankException)
            {
                _logger.LogFailure(bankException.Message);
                return false;
            }
        }

        public bool Transfer(int sourceBankAccountId, int destinationBankAccountId, decimal amount)
        {
            if (!TryGetBankAccount(sourceBankAccountId, out var sourceAccount)) return false;
            if (!TryGetBankAccount(destinationBankAccountId, out var destinationBankAccount)) return false;
            
            try
            {
                sourceAccount.Transfer(destinationBankAccount, amount);
                
                _bankRepository.Save(sourceAccount);
                _bankRepository.Save(destinationBankAccount);
                
                _logger.LogSuccess($"Transferred {amount} EUR from account {sourceBankAccountId} to account {destinationBankAccountId}.");
                
                return true;
            }
            catch (BankException bankException)
            {
                _logger.LogFailure(bankException.Message);
                return false;
            }            
        }

        private bool TryGetBankAccount(int bankAccountId, out BankAccount bankAccount)
        {
            bankAccount = _bankRepository.Get(bankAccountId);

            if (bankAccount != null) return true;
            
            _logger.LogFailure($"Bank account {bankAccountId} not found.");
            return false;
        }
    }
}