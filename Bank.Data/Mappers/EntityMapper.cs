using System.Collections.Generic;
using System.Linq;

namespace Bank.Data.Mappers
{
    public class EntityMapper : IEntityMapper
    {
        public IList<Domain.BankAccountSummary> ToDomain(IEnumerable<Entities.BankAccount> bankAccounts)
        {
            if (bankAccounts == null) return new List<Domain.BankAccountSummary>();
            
            return bankAccounts
                    .Select(x => new Domain.BankAccountSummary(
                                                    x.Id, 
                                                    x.Holder.Id,
                                                    $"{x.Holder.FirstName} {x.Holder.LastName}",
                                                    x.Balance,
                                                    x.MinBalance))
                    .ToList();
        }

        public Domain.BankAccount ToDomain(Entities.BankAccount bankAccount)
        {
            return new(
                    bankAccount.Id,
                    ToDomain(bankAccount.Holder),
                    bankAccount.Balance,
                    bankAccount.MinBalance,
                    ToDomain(bankAccount.Transactions));
        }

        public Entities.BankAccount ToEntity(Domain.BankAccount bankAccount)
        {
            return new()
            {
                Id = bankAccount.Id,
                Holder = ToEntity(bankAccount.Holder),
                Balance = bankAccount.Balance,
                MinBalance = bankAccount.MinBalance,
                Transactions = ToEntity(bankAccount.Transactions)
            };
        }

        public Domain.Person ToDomain(Entities.Person person)
        {
            return new(person.Id, person.FirstName, person.LastName);
        }

        public Entities.Person ToEntity(Domain.Person person)
        {
            return new()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
            };
        }

        private Entities.BankAccountTransaction ToEntity(Domain.BankAccountTransaction bankAccountTransaction)
        {
            switch (bankAccountTransaction)
            {
                case Domain.BankAccountDepositTransaction _:
                    return new Entities.BankAccountDepositTransaction
                    {
                        Amount = bankAccountTransaction.Amount, 
                        DateTime = bankAccountTransaction.DateTime
                    };
                case Domain.BankAccountWithdrawalTransaction _:
                    return new Entities.BankAccountWithdrawalTransaction
                    {
                        Amount = bankAccountTransaction.Amount, 
                        DateTime = bankAccountTransaction.DateTime
                    };
                case Domain.BankAccountTransferTransaction transferTransaction:
                    return new Entities.BankAccountTransferTransaction 
                    { 
                        SourceBankAccountId = transferTransaction.SourceBankAccountId,
                        DestinationBankAccountId = transferTransaction.DestinationBankAccountId, 
                        Amount = bankAccountTransaction.Amount, 
                        DateTime = bankAccountTransaction.DateTime
                    };
                default:
                    return null;
            }
        }

        private IList<Entities.BankAccountTransaction> ToEntity(IEnumerable<Domain.BankAccountTransaction> bankAccountTransactions)
        {
            return bankAccountTransactions != null 
                ? bankAccountTransactions.Select(ToEntity).ToList() 
                : new List<Entities.BankAccountTransaction>();
        }
        
        private Domain.BankAccountTransaction ToDomain(Entities.BankAccountTransaction bankAccountTransaction)
        {
            switch (bankAccountTransaction)
            {
                case Entities.BankAccountDepositTransaction _:
                    return new Domain.BankAccountDepositTransaction(bankAccountTransaction.Amount, bankAccountTransaction.DateTime);
                case Entities.BankAccountWithdrawalTransaction _:
                    return new Domain.BankAccountWithdrawalTransaction(bankAccountTransaction.Amount, bankAccountTransaction.DateTime);
                case Entities.BankAccountTransferTransaction transferTransaction:
                    return new Domain.BankAccountTransferTransaction( 
                                transferTransaction.SourceBankAccountId, 
                                transferTransaction.DestinationBankAccountId, 
                                bankAccountTransaction.Amount, 
                                bankAccountTransaction.DateTime);
                default:
                    return null;
            }
        }

        private IList<Domain.BankAccountTransaction> ToDomain(IList<Entities.BankAccountTransaction> bankAccountTransactions)
        {
            return bankAccountTransactions != null 
                ? bankAccountTransactions.Select(ToDomain).ToList() 
                : new List<Domain.BankAccountTransaction>();
        }
    }
}