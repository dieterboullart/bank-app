using System.Collections.Generic;

namespace Bank.Domain.Interfaces
{
    public interface IBankAccountRepository
    {
        IList<BankAccountSummary> GetSummaries();
        BankAccount Get(int accountId);
        void Save(BankAccount bankAccount);
    }
}