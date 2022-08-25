using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Data.Context;
using Bank.Data.Mappers;
using Bank.Data.Mappers.Interfaces;
using Bank.Data.Repositories.Interfaces;
using Bank.Domain.Models;

namespace Bank.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly BankContext _context;
        private readonly IEntityMapper _entityMapper;

        public PersonRepository(IEntityMapper entityMapper, BankContext context)
        {
            _entityMapper = entityMapper;
            _context = context;
        }

        public IList<Person> Find(string nameQuery)
        {
            return (
                from person in _context.Persons
                where person.Name.ToLower().Contains(nameQuery.ToLower())
                select _entityMapper.ToDomain(person)
            ).ToList();
        }

        public void Save(Person person)
        {
            var personCollection = _context.Persons;

            var entity = _entityMapper.ToEntity(person);

            if (personCollection.Any(x => x.Id == person.Id))
            {
                personCollection.Update(entity);
            }
            else
            {
                personCollection.Add(entity);
                // TODO: ???
                person.Id = entity.Id;
            }
            // TODO: Needed?
            // _context.SaveChanges();
        }
    }
}