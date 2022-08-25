using System;
using System.Transactions;
using Bank.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Data.Context;

public class BankContext : DbContext
{
    private string DbPath { get; }

    public DbSet<Person> Persons { get; set; }
    public DbSet<BankAccount> Accounts { get; set; }

    // Inherited transaction classes
    public DbSet<BankAccountTransaction> Transactions { get; set; }
    public DbSet<BankAccountDepositTransaction> DepositTransactions { get; set; }
    public DbSet<BankAccountWithdrawalTransaction> WithdrawalTransactions { get; set; }
    public DbSet<BankAccountTransferTransaction> TransferTransactions { get; set; }

    public BankContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "bank.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region BankAccount

        modelBuilder.Entity<BankAccount>().HasKey(e => e.Id);
        modelBuilder.Entity<BankAccount>().Property(e => e.MinBalance).IsRequired();
        modelBuilder.Entity<BankAccount>().Property(e => e.Balance).IsRequired().HasDefaultValue(0);
        modelBuilder.Entity<BankAccount>()
            .HasOne(e => e.Holder)
            .WithMany(e => e.Accounts)
            .HasForeignKey(e => e.HolderId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<BankAccount>()
            .HasMany(e => e.Transactions)
            .WithOne(e => e.Account)
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Person

        modelBuilder.Entity<Person>().HasKey(e => e.Id);
        modelBuilder.Entity<Person>().Property(e => e.FirstName).IsRequired().HasMaxLength(30);
        modelBuilder.Entity<Person>().Property(e => e.LastName).IsRequired().HasMaxLength(30);
        modelBuilder.Entity<Person>().Ignore(e => e.Name);

        #endregion

        #region Transactions

        modelBuilder.Entity<BankAccountTransaction>().HasKey(e => e.Id);
        modelBuilder.Entity<BankAccountTransaction>().Property(e => e.Amount).IsRequired();
        modelBuilder.Entity<BankAccountTransaction>().Property(e => e.DateTime).IsRequired();
        modelBuilder.Entity<BankAccountTransferTransaction>().Ignore(e => e.SourceBankAccount);
        modelBuilder.Entity<BankAccountTransferTransaction>().Ignore(e => e.DestinationBankAccount);

        #endregion
    }
}