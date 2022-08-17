using System.Collections.Generic;
using Bank.Domain.Models;

namespace Bank.Data.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        IList<Person> Find(string nameQuery);
        void Save(Person person);
    }
}