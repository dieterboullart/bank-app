using System.Collections.Generic;
using System.Linq;
using Bank.Data.Mappers.Interfaces;
using Bank.Domain.Models;

namespace Bank.Data.Mappers
{
    public class EntityMapper : IEntityMapper
    {
        public IList<BankAccountSummary> ToDomain(IEnumerable<Entities.BankAccount> bankAccounts)
        {
            if (bankAccounts == null) return new List<BankAccountSummary>();

            return bankAccounts
                .Select(x => new BankAccountSummary(
                    x.Id,
                    x.Holder.Id,
                    $"{x.Holder.FirstName} {x.Holder.LastName}",
                    x.Balance,
                    x.MinBalance))
                .ToList();
        }

        public BankAccount ToDomain(Entities.BankAccount bankAccount)
        {
            return new(
                bankAccount.Id,
                ToDomain(bankAccount.Holder),
                bankAccount.Balance,
                bankAccount.MinBalance,
                ToDomain(bankAccount.Transactions));
        }

        public Entities.BankAccount ToEntity(BankAccount bankAccount)
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

        public Person ToDomain(Entities.Person person)
        {
            return new(person.Id, person.FirstName, person.LastName);
        }

        public Entities.Person ToEntity(Person person)
        {
            return new()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
            };
        }

        private Entities.BankAccountTransaction ToEntity(BankAccountTransaction bankAccountTransaction)
        {
            switch (bankAccountTransaction)
            {
                case BankAccountDepositTransaction _:
                    return new Entities.BankAccountDepositTransaction
                    {
                        Id = bankAccountTransaction.Id,
                        Amount = bankAccountTransaction.Amount,
                        DateTime = bankAccountTransaction.DateTime
                    };
                case BankAccountWithdrawalTransaction _:
                    return new Entities.BankAccountWithdrawalTransaction
                    {
                        Id = bankAccountTransaction.Id,
                        Amount = bankAccountTransaction.Amount,
                        DateTime = bankAccountTransaction.DateTime
                    };
                case BankAccountTransferTransaction transferTransaction:
                    return new Entities.BankAccountTransferTransaction
                    {
                        Id = bankAccountTransaction.Id,
                        SourceBankAccountId = transferTransaction.SourceBankAccountId,
                        DestinationBankAccountId = transferTransaction.DestinationBankAccountId,
                        Amount = bankAccountTransaction.Amount,
                        DateTime = bankAccountTransaction.DateTime
                    };
                default:
                    return null;
            }
        }

        private IList<Entities.BankAccountTransaction> ToEntity(
            IEnumerable<BankAccountTransaction> bankAccountTransactions)
        {
            return bankAccountTransactions != null
                ? bankAccountTransactions.Select(ToEntity).ToList()
                : new List<Entities.BankAccountTransaction>();
        }

        private BankAccountTransaction ToDomain(Entities.BankAccountTransaction bankAccountTransaction)
        {
            switch (bankAccountTransaction)
            {
                case Entities.BankAccountDepositTransaction _:
                    return new BankAccountDepositTransaction(bankAccountTransaction.Id, bankAccountTransaction.Amount,
                        bankAccountTransaction.DateTime);
                case Entities.BankAccountWithdrawalTransaction _:
                    return new BankAccountWithdrawalTransaction(bankAccountTransaction.Id,
                        bankAccountTransaction.Amount, bankAccountTransaction.DateTime);
                case Entities.BankAccountTransferTransaction transferTransaction:
                    return new BankAccountTransferTransaction(
                        bankAccountTransaction.Id,
                        transferTransaction.SourceBankAccountId,
                        transferTransaction.DestinationBankAccountId,
                        bankAccountTransaction.Amount,
                        bankAccountTransaction.DateTime);
                default:
                    return null;
            }
        }

        private IList<BankAccountTransaction> ToDomain(IList<Entities.BankAccountTransaction> bankAccountTransactions)
        {
            return bankAccountTransactions != null
                ? bankAccountTransactions.Select(ToDomain).ToList()
                : new List<BankAccountTransaction>();
        }
    }
}