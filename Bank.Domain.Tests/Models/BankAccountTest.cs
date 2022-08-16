using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Domain;
using FluentAssertions;
using Xunit;

namespace Bank.Tests.Models
{
    public class BankAccountTest
    {
        private readonly Person _holder = new("Jan", "Janssen");
        private readonly Person _otherHolder = new("Peter", "Peterssen");
        
        [Fact]
        public void Constructor_WithId()
        {
            // Act
            var transactions = new List<BankAccountTransaction>
            {
                new BankAccountDepositTransaction(10, DateTime.Now),
                new BankAccountWithdrawalTransaction(5, DateTime.Now),
            };
            
            var sut = new BankAccount(1, _holder, 1000, -500, transactions);
            
            // Assert
            sut.Id.Should().Be(1);
            sut.Holder.Should().Be(_holder);
            sut.Balance.Should().Be(1000);
            sut.MinBalance.Should().Be(-500);
            sut.Transactions.Should().Equal(transactions.First(), transactions.Last());
        }
        
        [Fact]
        public void Constructor_NoOptionalParameters()
        {
            // Act
            var sut = new BankAccount(_holder);
            
            // Assert
            sut.Id.Should().Be(0);
            sut.Holder.Should().Be(_holder);
            sut.Balance.Should().Be(0);
            sut.MinBalance.Should().Be(0);
            sut.Transactions.Should().BeEmpty();
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public void Constructor_MinBalancePositive(int minBalance)
        {
            // Act
            Action act = () => new BankAccount(new Person("Jan", "Janssen"), 1000, minBalance);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Minimum balance cannot be higher than 0.");
        }        
        
        [Fact]
        public void Constructor_BalanceLowerThanMinBalance()
        {
            // Act
            Action act = () => new BankAccount(new Person("Jan", "Janssen"), -501, -500);
            
            // Act/Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Balance cannot be lower than minimum balance.");
        }

        [Fact]
        public void Deposit()
        {
            // Arrange
            var sut = new BankAccount(_holder, 1000);
            var dateTime = new DateTime(2020, 8, 6);
            
            // Act
            sut.Deposit(500.5m, dateTime);
            
            // Assert
            sut.Balance.Should().Be(1500.5m);
            sut.Transactions.Count.Should().Be(1);
            sut.Transactions.First().Should().BeOfType(typeof(BankAccountDepositTransaction));
            sut.Transactions.First().Amount.Should().Be(500.5m);
            sut.Transactions.First().DateTime.Should().Be(dateTime);
        }
        
        [Fact]
        public void Deposit_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(new Person("Jan", "Janssen"), 1000);
            
            // Act
            Action act = () => sut.Deposit(-500, DateTime.Now);
            
            // Assert
            act.Should().Throw<BankException>().WithMessage("Amount cannot be negative.");
            sut.Transactions.Should().BeEmpty();
        }        
        
        [Fact]
        public void Withdraw()
        {
            // Arrange
            var sut = new BankAccount(_holder, 1000);
            var dateTime = new DateTime(2020, 8, 6);
            
            // Act
            sut.Withdraw(999, dateTime);
            
            // Assert
            sut.Balance.Should().Be(1);
            sut.Transactions.Count.Should().Be(1);
            sut.Transactions.First().Should().BeOfType(typeof(BankAccountWithdrawalTransaction));
            sut.Transactions.First().Amount.Should().Be(999);
            sut.Transactions.First().DateTime.Should().Be(dateTime);
        }
        
        [Fact]
        public void Withdraw_MinBalanceNegative()
        {
            // Arrange
            var sut = new BankAccount(_holder, 500, -500);
            var dateTime = new DateTime(2020, 8, 6);
            
            // Act
            sut.Withdraw(1000, dateTime);
            
            // Assert
            sut.Balance.Should().Be(-500);
            sut.Transactions.Count.Should().Be(1);
            sut.Transactions.First().Should().BeOfType(typeof(BankAccountWithdrawalTransaction));
            sut.Transactions.First().Amount.Should().Be(1000);
            sut.Transactions.First().DateTime.Should().Be(dateTime);
        }        
        
        [Fact]
        public void Withdraw_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(_holder, 1000);
            
            // Act
            Action act = () => sut.Deposit(-500, DateTime.Now);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Amount cannot be negative.");
            sut.Transactions.Should().BeEmpty();
        }      
        
        [Fact]
        public void Withdraw_BalanceInsufficient()
        {
            // Arrange
            var sut = new BankAccount(_holder, 1000, -500);
            
            // Act/Assert
            Action act = () => sut.Withdraw(1501, DateTime.Now); 
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Balance insufficient.");
        }
        
        [Fact]
        public void Transfer()
        {
            // Arrange
            var sut = new BankAccount(_holder, 1000) {Id = 1};
            var destinationBankAccount = new BankAccount(_otherHolder, 500) {Id = 2};
            
            var dateTime = new DateTime(2020, 8, 6);
            
            // Act
            sut.Transfer(destinationBankAccount, 200, dateTime);
            
            // Assert
            sut.Balance.Should().Be(800);
            destinationBankAccount.Balance.Should().Be(700);
            
            sut.Transactions.Count.Should().Be(1);
            sut.Transactions.First().Should().BeOfType<BankAccountTransferTransaction>();
            sut.Transactions.OfType<BankAccountTransferTransaction>().First().SourceBankAccountId.Should().Be(sut.Id);
            sut.Transactions.OfType<BankAccountTransferTransaction>().First().DestinationBankAccountId.Should().Be(destinationBankAccount.Id);
            sut.Transactions.OfType<BankAccountTransferTransaction>().First().Amount.Should().Be(200);
            sut.Transactions.OfType<BankAccountTransferTransaction>().First().DateTime.Should().Be(dateTime);

            destinationBankAccount.Transactions.Count.Should().Be(1);
            destinationBankAccount.Transactions.First().Should().BeOfType<BankAccountTransferTransaction>();
            destinationBankAccount.Transactions.OfType<BankAccountTransferTransaction>().First().SourceBankAccountId.Should().Be(sut.Id);
            destinationBankAccount.Transactions.OfType<BankAccountTransferTransaction>().First().DestinationBankAccountId.Should().Be(destinationBankAccount.Id);
            destinationBankAccount.Transactions.OfType<BankAccountTransferTransaction>().First().Amount.Should().Be(200);
            destinationBankAccount.Transactions.OfType<BankAccountTransferTransaction>().First().DateTime.Should().Be(dateTime);
        }
        
        [Fact]
        public void Transfer_SourceMinBalanceNegative()
        {
            // Arrange
            var sut = new BankAccount(_holder, 1000, -700);
            var destinationBankAccount = new BankAccount(_otherHolder, 500);

            var dateTime = new DateTime(2020, 8, 6);
            
            // Act
            sut.Transfer(destinationBankAccount, 1700, dateTime);
            
            // Assert
            sut.Balance.Should().Be(-700);
            destinationBankAccount.Balance.Should().Be(2200);
        }        
        
        [Fact]
        public void Transfer_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(_holder, 1000);
            var destinationBankAccount = new BankAccount(_otherHolder, 500);

            var dateTime = new DateTime(2020, 8, 6);
            
            // Act
            Action act = () => sut.Transfer(destinationBankAccount, -500, dateTime);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Amount cannot be negative.");
        }      
        
        [Fact]
        public void Transfer_SourceBalanceInsufficient()
        {
            // Arrange
            var sut = new BankAccount(_holder, 1000, -500);
            var destinationBankAccount = new BankAccount(_otherHolder, 500);
            
            // Act
            Action act = () => sut.Transfer(destinationBankAccount, 1501, DateTime.Now);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Balance insufficient.");
        }                
    }
}

