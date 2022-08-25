using System.Collections.Generic;
using System.Linq;
using Bank.Data.Context;
using Bank.Data.Mappers;
using Bank.Data.Mappers.Interfaces;
using Bank.Data.Repositories.Interfaces;
using Bank.Domain.Models;
using LiteDB;
using Microsoft.EntityFrameworkCore;

namespace Bank.Data.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly IEntityMapper _entityMapper;
        private readonly BankContext _context;
        
        public BankAccountRepository(IEntityMapper entityMapper, BankContext context)
        {
            _entityMapper = entityMapper;
            _context = context;
        }

        public IList<BankAccountSummary> GetSummaries()
        {
            var entities = _context.Accounts
                .Include(x => x.Holder)
                .Include(x => x.Transactions)
                .AsNoTracking()
                .AsEnumerable();

            return _entityMapper.ToDomain(entities);                
        }

        public BankAccount Get(int accountId)
        {
            var entity = _context.Accounts
                .Include(e => e.Holder)
                .Include(x => x.Transactions)
                .AsNoTracking()
                .SingleOrDefault(e => e.Id == accountId);

            return _entityMapper.ToDomain(entity);
        }

        public void Save(BankAccount bankAccount)
        {
            var entity = _entityMapper.ToEntity(bankAccount);
            
            var bankAccountCollection = _context.Accounts;
            
            if (bankAccountCollection.Any(x => x.Id == bankAccount.Id))
            {
                bankAccountCollection.Update(entity);
            }
            else
            {
                bankAccountCollection.Add(entity);
                // TODO: ???
                bankAccount.Id = entity.Id;
            }

            _context.SaveChanges();
        }
    }
}