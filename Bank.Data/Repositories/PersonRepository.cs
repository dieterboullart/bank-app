using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Data.Mappers;
using Bank.Data.Mappers.Interfaces;
using Bank.Data.Repositories.Interfaces;
using Bank.Domain.Models;

namespace Bank.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IEntityMapper _entityMapper;

        public PersonRepository(IEntityMapper entityMapper)
        {
            _entityMapper = entityMapper;
        }
        
        public IList<Person> Find(string nameQuery)
        {
            using var db = LiteDBProvider.CreateDatabase();

            return db.GetCollection<Entities.Person>()
                        .Find(x => x.Name.ToLower().Contains(nameQuery.ToLower()))
                        .Select(x => _entityMapper.ToDomain(x))
                        .ToList();
        }

        public void Save(Person person)
        {
            using var db = LiteDBProvider.CreateDatabase();

            var personCollection = db.GetCollection<Entities.Person>();

            var entity = _entityMapper.ToEntity(person);
            
            if (personCollection.Exists(x => x.Id == person.Id))
            {
                personCollection.Update(entity);
            }
            else
            {
                personCollection.Insert(entity);
                person.Id = entity.Id;
            }        
        }
    }
}