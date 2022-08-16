using System.Collections.Generic;

namespace Bank.Domain.Interfaces
{
    public interface IPersonRepository
    {
        IList<Person> Find(string nameQuery);
        void Save(Person person);
    }
}