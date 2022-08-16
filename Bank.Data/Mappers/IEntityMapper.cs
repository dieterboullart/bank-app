using System.Collections.Generic;

namespace Bank.Data.Mappers
{
    public interface IEntityMapper
    {
        IList<Domain.BankAccountSummary> ToDomain(IEnumerable<Entities.BankAccount> bankAccounts);
        Domain.BankAccount ToDomain(Entities.BankAccount bankAccount);
        Entities.BankAccount ToEntity(Domain.BankAccount bankAccount);
        Domain.Person ToDomain(Entities.Person person);
        Entities.Person ToEntity(Domain.Person person);
    }
}