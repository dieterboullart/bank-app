using Bank.BL.Services.Interfaces;
using Bank.Data.Repositories.Interfaces;
using Bank.Domain.Models;
using Bank.Shared.Logging.Interfaces;
using Bank.Shared.Utils.Clock.Interfaces;

namespace Bank.BL.Services
{
    public class BankService : IBankService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ILogger _logger;
        private readonly IClock _clock;

        public BankService(IBankAccountRepository bankAccountRepository, ILogger logger, IClock clock)
        {
            _bankAccountRepository = bankAccountRepository;
            _logger = logger;
            _clock = clock;
        }

        public IList<BankAccountSummary> GetSummaries()
        {
            return _bankAccountRepository.GetSummaries().OrderBy(x => x.Id).ToList();
        }

        public BankAccount GetDetail(int accountId)
        {
            var bankAccount = _bankAccountRepository.Get(accountId);
            if (bankAccount == null)
            {
                _logger.LogFailure($"Bank account with id {accountId} not found.");
            }

            return bankAccount;
        }

        public bool Deposit(int bankAccountId, decimal amount)
        {
            if (!TryGetBankAccount(bankAccountId, out var bankAccount))
            {
                return false;
            }
            
            try
            {
                bankAccount.Deposit(amount, _clock.CurrentUtcTime);
                _bankAccountRepository.Save(bankAccount);
                
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
                bankAccount.Withdraw(amount, _clock.CurrentUtcTime);
                _bankAccountRepository.Save(bankAccount);
                
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
                sourceAccount.Transfer(destinationBankAccount, amount, _clock.CurrentUtcTime);
                
                _bankAccountRepository.Save(sourceAccount);
                _bankAccountRepository.Save(destinationBankAccount);
                
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
            bankAccount = _bankAccountRepository.Get(bankAccountId);

            if (bankAccount != null) return true;
            
            _logger.LogFailure($"Bank account {bankAccountId} not found.");
            return false;
        }
    }
}