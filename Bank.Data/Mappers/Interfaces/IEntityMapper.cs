using System.Collections.Generic;
using Bank.Domain.Models;

namespace Bank.Data.Mappers.Interfaces
{
    public interface IEntityMapper
    {
        IList<BankAccountSummary> ToDomain(IEnumerable<Entities.BankAccount> bankAccounts);
        BankAccount ToDomain(Entities.BankAccount bankAccount);
        Entities.BankAccount ToEntity(BankAccount bankAccount);
        Person ToDomain(Entities.Person person);
        Entities.Person ToEntity(Person person);
    }
}