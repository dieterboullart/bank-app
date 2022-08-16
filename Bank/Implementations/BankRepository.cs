using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Bank.Interfaces;
using Bank.Models;

namespace Bank.Implementations
{
    public class BankRepository : IBankRepository
    {
        private const string BankFilename = "bank.json";

        private List<BankAccount> _bankAccounts;
        
        public IList<BankAccount> GetAll()
        {
            LoadData();
            return _bankAccounts.ToList();
        }

        public BankAccount Get(int accountId)
        {
            LoadData();
            return _bankAccounts.SingleOrDefault(x => x.AccountId == accountId);
        }

        public void Save(BankAccount bankAccount)
        {
            LoadData();

            var existingBankAccount = Get(bankAccount.AccountId);

            if (existingBankAccount != null)
            {
                _bankAccounts.Remove(existingBankAccount);
            }
            
            _bankAccounts.Add(bankAccount);
            SaveAll();
        }

        private void LoadData()
        {
            if (!File.Exists(BankFilename))
            {
                Seed();
                SaveAll();
            }
            else
            {
                _bankAccounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(BankFilename));
            }
        }

        private void SaveAll()
        {
            File.WriteAllText(BankFilename, JsonSerializer.Serialize(_bankAccounts));
        }

        private void Seed()
        {
            _bankAccounts = new List<BankAccount>
            {
                new(1, "Florian Goeteyn", 1023),
                new(2, "Pieter Remerie", 768, -500),
                new(3, "Bart Bruynooghe", 1540, 0),
                new(4, "Jelle Vandendriesche", 2088, -250),
                new(5, "Dieter Boullart")
            };
        }
    }
}