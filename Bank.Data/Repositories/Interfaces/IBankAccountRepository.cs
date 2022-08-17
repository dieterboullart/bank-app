using System.Collections.Generic;
using Bank.Domain.Models;

namespace Bank.Data.Repositories.Interfaces
{
    public interface IBankAccountRepository
    {
        IList<BankAccountSummary> GetSummaries();
        BankAccount Get(int accountId);
        void Save(BankAccount bankAccount);
    }
}