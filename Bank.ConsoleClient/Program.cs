using System;
using System.Collections.Generic;
using System.Linq;
using Bank.BL.Services;
using Bank.BL.Services.Interfaces;
using Bank.ConsoleClient.Startup;
using Bank.Data.Mappers;
using Bank.Data.Repositories;
using Bank.Domain;
using Bank.Domain.Models;
using DryIoc;

namespace Bank.ConsoleClient
{
    internal static class Program
    {
        private static IContainer _container;

        public static void Main()
        {
            // Setup DI container
            _container = DryIocConfiguration.ConfigureIoc();

            // Start program execution
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
            using var resolver = _container.OpenScope();
            var bankService = resolver.Resolve<IBankService>();
            foreach (var bankAccount in bankService.GetSummaries())
            {
                Console.WriteLine(
                    $"Account id: {bankAccount.Id}. Holder: {bankAccount.HolderName}. Minimum balance: {bankAccount.MinBalance} EUR. Current balance: {bankAccount.Balance}) EUR.");
            }
        }

        private static void ShowDetail()
        {
            Console.Write("Account id? ");
            ReadIntFromConsole(out var accountId);
            using var resolver = _container.OpenScope();
            var bankService = resolver.Resolve<IBankService>();
            var bankAccount = bankService.GetDetail(accountId);

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
                            Console.WriteLine(
                                $"TRANSFER - amount: {transferTransaction.Amount}. Destination account id: {transferTransaction.DestinationBankAccountId}.");
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

            using var resolver = _container.OpenScope();
            var bankService = resolver.Resolve<IBankService>();
            bankService.Deposit(accountId, amount);
        }

        private static void Withdraw()
        {
            Console.Write("Account id? ");
            ReadIntFromConsole(out var accountId);
            Console.Write("Amount to withdraw? ");
            ReadIntFromConsole(out var amount);
            Console.WriteLine();

            using var resolver = _container.OpenScope();
            var bankService = resolver.Resolve<IBankService>();
            bankService.Withdraw(accountId, amount);
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

            using var resolver = _container.OpenScope();
            var bankService = resolver.Resolve<IBankService>();
            bankService.Transfer(sourceAccountId, destinationAccountId, amount);
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
    }
}