using System.Collections.Generic;
using Bank.Data.Mappers;
using Bank.Domain.Interfaces;
using LiteDB;
using LiteDB.Engine;

namespace Bank.Data.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly IEntityMapper _entityMapper;
        
        public BankAccountRepository(IEntityMapper entityMapper)
        {
            _entityMapper = entityMapper;
            SetupDB();
        }

        private void SetupDB()
        {
            BsonMapper.Global.Entity<Entities.BankAccount>()
                .DbRef(x => x.Holder);

            RunMigrations();
        }

        private static void RunMigrations()
        {
            using var db = LiteDBProvider.CreateDatabase();

            if (db.UserVersion == 0)
            {
                var collection = db.GetCollection(nameof(Entities.BankAccount));
                foreach (var bsonDocument in collection.FindAll())
                {
                    bsonDocument["holder"] = bsonDocument["owner"];
                    bsonDocument["owner"] = null;
                    collection.Update(bsonDocument);
                }

                db.UserVersion = 1;
            }
        }

        public IList<Domain.BankAccountSummary> GetSummaries()
        {
            using var db = LiteDBProvider.CreateDatabase();

            var entities = db.GetCollection<Entities.BankAccount>()
                                                        .Include(x => x.Holder)
                                                        .FindAll();

            return _entityMapper.ToDomain(entities);                
        }

        public Domain.BankAccount Get(int accountId)
        {
            using var db = LiteDBProvider.CreateDatabase();

            var entity = db.GetCollection<Entities.BankAccount>()
                                        .FindById(accountId);

            return _entityMapper.ToDomain(entity);
        }

        public void Save(Domain.BankAccount bankAccount)
        {
            var entity = _entityMapper.ToEntity(bankAccount);

            using var db = LiteDBProvider.CreateDatabase();
            
            var bankAccountCollection = db.GetCollection<Entities.BankAccount>();
            
            if (bankAccountCollection.Exists(x => x.Id == bankAccount.Id))
            {
                bankAccountCollection.Update(entity);
            }
            else
            {
                bankAccountCollection.Insert(entity);
                bankAccount.Id = entity.Id;
            }
        }
    }
}