using Bank.Domain.Models;

namespace Bank.BL.Services.Interfaces
{
    public interface IBankService
    {
        IList<BankAccountSummary> GetSummaries();
        BankAccount GetDetail(int accountId);
        bool Deposit(int bankAccountId, decimal amount);
        bool Withdraw(int bankAccountId, decimal amount);
        bool Transfer(int sourceBankAccountId, int destinationBankAccountId, decimal amount);
    }
}