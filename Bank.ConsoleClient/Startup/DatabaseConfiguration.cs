using System.Collections.Generic;
using Bank.Data.Repositories.Interfaces;
using Bank.Domain.Models;
using DryIoc;

namespace Bank.ConsoleClient.Startup;

public static class DatabaseConfiguration
{
    public static void SeedDatabase(IContainer container)
    {
        var personRepository = container.Resolve<IPersonRepository>();
        var bankAccountRepository = container.Resolve<IBankAccountRepository>();
        
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