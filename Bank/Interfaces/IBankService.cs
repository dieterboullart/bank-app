using System.Collections.Generic;
using Bank.Models;

namespace Bank.Interfaces
{
    public interface IBankService
    {
        IList<BankAccount> GetAllAccounts();
        bool Deposit(int bankAccountId, decimal amount);
        bool Withdraw(int bankAccountId, decimal amount);
        bool Transfer(int sourceBankAccountId, int destinationBankAccountId, decimal amount);
    }
}