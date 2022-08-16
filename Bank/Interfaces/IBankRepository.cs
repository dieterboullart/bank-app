using System.Collections.Generic;
using Bank.Models;

namespace Bank.Interfaces
{
    public interface IBankRepository
    {
        IList<BankAccount> GetAll();
        BankAccount Get(int accountId);
        void Save(BankAccount bankAccount);
    }
}