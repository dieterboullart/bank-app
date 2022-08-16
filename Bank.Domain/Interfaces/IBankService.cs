using System.Collections.Generic;

namespace Bank.Domain.Interfaces
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