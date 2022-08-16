using System;
using Bank;
using Bank.Implementations;
using Bank.Interfaces;

namespace UnitTestingTutorial
{
    static class Program
    {
        private static IBankService _bankService;
        
        public static void Main()
        {
            _bankService = new BankService(new BankRepository(), new ConsoleLogger());
            
            ShowMenu();
        }

        private static void ShowMenu()
        {
            var input = 0;

            do
            {
                Console.WriteLine("=========================");
                Console.WriteLine("        BANK menu        ");
                Console.WriteLine("=========================");
                Console.WriteLine("1 List all accounts");
                Console.WriteLine("2 Deposit");
                Console.WriteLine("3 Withdraw");
                Console.WriteLine("4 Transfer");
                Console.WriteLine("5 Quit");

                int.TryParse(Console.ReadLine(), out input);

                Console.WriteLine();

                switch (input)
                {
                    case 1:
                        ListAll();
                        break;
                    case 2:
                        Deposit();
                        break;
                    case 3:
                        Withdraw();
                        break;
                    case 4:
                        Transfer();
                        break;
                }

                Console.WriteLine();
            } while (input != 5);
        }

        private static void ListAll()
        {
            foreach (var bankAccount in _bankService.GetAllAccounts())
            {
                Console.WriteLine($"Account id: {bankAccount.AccountId}. Owner: {bankAccount.Owner}. Minimum balance: {bankAccount.MinBalance} EUR. Current balance: {bankAccount.Balance}) EUR.");
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
    }
}