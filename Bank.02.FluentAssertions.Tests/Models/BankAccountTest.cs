using System;
using Bank.Models;
using FluentAssertions;
using Xunit;

namespace Bank.FluentAssertions.Tests.Models
{
    public class BankAccountTest
    {
        [Fact]
        public void Constructor()
        {
            // Act
            var sut = new BankAccount(1, "Jan", 1000, -500);
            
            // Assert
            sut.AccountId.Should().Be(1);
            sut.Owner.Should().Be("Jan");
            sut.Balance.Should().Be(1000);
            sut.MinBalance.Should().Be(-500);
        }
        
        [Fact]
        public void Constructor_NoOptionalParameters()
        {
            // Act
            var sut = new BankAccount(1, "Jan");
            
            // Assert
            sut.AccountId.Should().Be(1);
            sut.Owner.Should().Be("Jan");
            sut.Balance.Should().Be(0);
            sut.MinBalance.Should().Be(0);            
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public void Constructor_MinBalancePositive(int minBalance)
        {
            // Act
            Action act = () => new BankAccount(1, "Jan", 1000, minBalance);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Minimum balance cannot be higher than 0.");
        }        
        
        [Fact]
        public void Constructor_BalanceLowerThanMinBalance()
        {
            // Act
            Action act = () => new BankAccount(1, "Jan", -501, -500);
            
            // Act/Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Balance cannot be lower than minimum balance.");
        }

        [Fact]
        public void Deposit()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            
            // Act
            sut.Deposit(500.5m);
            
            // Assert
            sut.Balance.Should().Be(1500.5m);
        }
        
        [Fact]
        public void Deposit_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            
            // Act
            Action act = () => sut.Deposit(-500);
            
            // Assert
            act.Should().Throw<BankException>().WithMessage("Amount cannot be negative.");
        }        
        
        [Fact]
        public void Withdraw()
        {
            // Arrange
            var sut = new BankAccount(999, "Jan", 1000);
            
            // Act
            sut.Withdraw(999);
            
            // Assert
            sut.Balance.Should().Be(1);
        }
        
        [Fact]
        public void Withdraw_MinBalanceNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 500, -500);
            
            // Act
            sut.Withdraw(1000);
            
            // Assert
            sut.Balance.Should().Be(-500);
        }        
        
        [Fact]
        public void Withdraw_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            
            // Act
            Action act = () => sut.Deposit(-500);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Amount cannot be negative.");
        }      
        
        [Fact]
        public void Withdraw_BalanceInsufficient()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000, -500);
            
            // Act
            Action act = () => sut.Withdraw(1501);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Balance insufficient.");
        }
        
        [Fact]
        public void Transfer()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            var destinationBankAccount = new BankAccount(2, "Piet", 500);
            
            // Act
            sut.Transfer(destinationBankAccount, 200);
            
            // Assert
            sut.Balance.Should().Be(800);
            destinationBankAccount.Balance.Should().Be(700);
        }
        
        [Fact]
        public void Transfer_SourceMinBalanceNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000, -700);
            var destinationBankAccount = new BankAccount(2, "Piet", 500);
            
            // Act
            sut.Transfer(destinationBankAccount, 1700);
            
            // Assert
            sut.Balance.Should().Be(-700);
            destinationBankAccount.Balance.Should().Be(2200);
        }        
        
        [Fact]
        public void Transfer_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            var destinationBankAccount = new BankAccount(2, "Piet", 500);
            
            // Act
            Action act = () => sut.Transfer(destinationBankAccount, -500);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Amount cannot be negative.");
        }      
        
        [Fact]
        public void Transfer_SourceBalanceInsufficient()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000, -500);
            var destinationBankAccount = new BankAccount(2, "Piet", 500);
            
            // Act
            Action act = () => sut.Transfer(destinationBankAccount, 1501);
            
            // Assert
            act.Should().Throw<BankException>().And.Message.Should().Be("Balance insufficient.");
        }                
    }
}

