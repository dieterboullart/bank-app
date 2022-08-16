using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Data.Mappers;
using Bank.Data.Repositories;
using Bank.Domain;
using Bank.Domain.Implementations;
using Bank.Domain.Interfaces;

namespace Bank.ConsoleClient
{
    internal static class Program
    {
        private static IBankService _bankService;
        
        public static void Main()
        {
            var bankRepository = new BankAccountRepository(new EntityMapper());
            var personRepository = new PersonRepository(new EntityMapper());
            
            // Seed the repo, comment next line out after the first time the program is run
            Seed(bankRepository, personRepository);
                
            _bankService = new BankService(bankRepository, new ConsoleLogger(), new SystemClock());
            
            ShowMenu();
        }

        private static void ShowMenu()
        {
            int input;

            do
            {
                Console.WriteLine("=========================");
                Console.WriteLine("        BANK menu        ");
                Console.WriteLine("=========================");
                Console.WriteLine("1 List all accounts");
                Console.WriteLine("2 Show account detail");
                Console.WriteLine("3 Deposit");
                Console.WriteLine("4 Withdraw");
                Console.WriteLine("5 Transfer");
                Console.WriteLine("6 Quit");

                int.TryParse(Console.ReadLine(), out input);

                Console.WriteLine();

                switch (input)
                {
                    case 1:
                        ListAll();
                        break;
                    case 2:
                        ShowDetail();
                        break;
                    case 3:
                        Deposit();
                        break;
                    case 4:
                        Withdraw();
                        break;
                    case 5:
                        Transfer();
                        break;
                }

                Console.WriteLine();
            } while (input != 6);
        }
        
        private static void ListAll()
        {
            Console.WriteLine();
            
            foreach (var bankAccount in _bankService.GetSummaries())
            {
                Console.WriteLine($"Account id: {bankAccount.Id}. Holder: {bankAccount.HolderName}. Minimum balance: {bankAccount.MinBalance} EUR. Current balance: {bankAccount.Balance}) EUR.");
            }
        }
        
        private static void ShowDetail()
        {
            Console.Write("Account id? ");
            ReadIntFromConsole(out var accountId);
            var bankAccount = _bankService.GetDetail(accountId);

            if (bankAccount is null) return;
            
            Console.WriteLine();
            Console.WriteLine($"Holder:          {bankAccount.Holder}");
            Console.WriteLine($"Minimum balance: {bankAccount.MinBalance}");
            Console.WriteLine($"Current balance: {bankAccount.Balance}");
            Console.Write($"Transactions   : ");

            if (bankAccount.Transactions.Any())
            {
                Console.WriteLine();
            
                var counter = 1;
                foreach (var transaction in bankAccount.Transactions)
                {
                    Console.Write($"     {counter++} {transaction.DateTime} ");
                    switch (transaction)
                    {
                        case BankAccountDepositTransaction _:
                            Console.WriteLine($"DEPOSIT  - amount: {transaction.Amount}.");
                            break;
                        case BankAccountWithdrawalTransaction _:
                            Console.WriteLine($"WITHDRAW - amount: {transaction.Amount}.");
                            break;
                        case BankAccountTransferTransaction transferTransaction:
                            Console.WriteLine($"TRANSFER - amount: {transferTransaction.Amount}. Destination account id: {transferTransaction.DestinationBankAccountId}.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("no transactions yet");
            }
        }        

        private static void Deposit()
        {
            Console.Write("Account id? ");
            ReadIntFromConsole(out var accountId);
            Console.Write("Amount to deposit? ");
            ReadIntFromConsole(out var amount);
            Console.WriteLine();

            _bankService.Deposit(accountId, amount);
        }
        
        private static void Withdraw()
        {
            Console.Write("Account id? ");
            ReadIntFromConsole(out var accountId);
            Console.Write("Amount to withdraw? ");
            ReadIntFromConsole(out var amount);
            Console.WriteLine();

            _bankService.Withdraw(accountId, amount);
        }
        
        private static void Transfer()
        {
            Console.Write("Source account id? ");
            ReadIntFromConsole(out var sourceAccountId);
            Console.Write("Destination account id? ");
            ReadIntFromConsole(out var destinationAccountId);
            Console.Write("Amount to transfer? ");
            ReadIntFromConsole(out var amount);
            Console.WriteLine();

            _bankService.Transfer(sourceAccountId, destinationAccountId, amount);
        }
     
        private static void ReadIntFromConsole(out int value)
        {
            value = 0;
            var parseSuccess = false;
            
            while (!parseSuccess)
            {
                parseSuccess = int.TryParse(Console.ReadLine(), out value);
            }
        }        
        
        private static void Seed(IBankAccountRepository bankAccountRepository, IPersonRepository personRepository)
        {
            var florian = new Person("Florian", "Goeteyn");
            var pieter = new Person("Pieter", "Remerie");
            var bart = new Person("Bart", "Bruynooghe");
            var jelle = new Person("Jelle", "Vandendriesche");

            personRepository.Save(florian);
            personRepository.Save(pieter);
            personRepository.Save(bart);
            personRepository.Save(jelle);
            
            var bankAccounts = new List<BankAccount>
            {
                new(florian, 1023),
                new(pieter, 768, -500),
                new(bart, 1540),
                new(jelle, 2088, -250),
                new(jelle)
            };
            
            foreach (var bankAccount in bankAccounts)
            {
                bankAccountRepository.Save(bankAccount);
            }
        }        
    }
}