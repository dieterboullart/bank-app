using Bank.Models;
using Xunit;

namespace Bank.Introduction.Tests.Models
{
    public class BankAccountTest
    {
        [Fact]
        public void Constructor()
        {
            // Act
            var sut = new BankAccount(1, "Jan", 1000, -500);
            
            // Assert
            Assert.Equal(1, sut.AccountId);
            Assert.Equal("Jan", sut.Owner);
            Assert.Equal(1000, sut.Balance);
            Assert.Equal(-500, sut.MinBalance);
        }
        
        [Fact]
        public void Constructor_NoOptionalParameters()
        {
            // Act
            var sut = new BankAccount(1, "Jan");
            
            // Assert
            Assert.Equal(1, sut.AccountId);
            Assert.Equal("Jan", sut.Owner);
            Assert.Equal(0, sut.Balance);
            Assert.Equal(0, sut.MinBalance);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public void Constructor_MinBalancePositive(int minBalance)
        {
            // Act/Assert
            var exception = Assert.Throws<BankException>(() => new BankAccount(1, "Jan", 1000, minBalance));
            
            Assert.Equal("Minimum balance cannot be higher than 0.", exception.Message);
        }        
        
        [Fact]
        public void Constructor_BalanceLowerThanMinBalance()
        {
            // Act/Assert
            var exception = Assert.Throws<BankException>(() => new BankAccount(1, "Jan", -501, -500));
            
            Assert.Equal("Balance cannot be lower than minimum balance.", exception.Message);
        }

        [Fact]
        public void Deposit()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            
            // Act
            sut.Deposit(500.5m);
            
            // Assert
            Assert.Equal(1500.5m, sut.Balance);
        }
        
        [Fact]
        public void Deposit_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            
            // Act/Assert
            var exception = Assert.Throws<BankException>(() => sut.Deposit(-500));
            
            Assert.Equal("Amount cannot be negative.", exception.Message);
        }        
        
        [Fact]
        public void Withdraw()
        {
            // Arrange
            var sut = new BankAccount(999, "Jan", 1000);
            
            // Act
            sut.Withdraw(999);
            
            // Assert
            Assert.Equal(1, sut.Balance);
        }
        
        [Fact]
        public void Withdraw_MinBalanceNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 500, -500);
            
            // Act
            sut.Withdraw(1000);
            
            // Assert
            Assert.Equal(-500, sut.Balance);
        }        
        
        [Fact]
        public void Withdraw_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            
            // Act/Assert
            var exception = Assert.Throws<BankException>(() => sut.Deposit(-500));
            
            Assert.Equal("Amount cannot be negative.", exception.Message);
        }      
        
        [Fact]
        public void Withdraw_BalanceInsufficient()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000, -500);
            
            // Act/Assert
            var exception = Assert.Throws<BankException>(() => sut.Withdraw(1501));
            
            Assert.Equal("Balance insufficient.", exception.Message);
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
            Assert.Equal(800, sut.Balance);
            Assert.Equal(700, destinationBankAccount.Balance);
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
            Assert.Equal(-700, sut.Balance);
            Assert.Equal(2200, destinationBankAccount.Balance);
        }        
        
        [Fact]
        public void Transfer_AmountNegative()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000);
            var destinationBankAccount = new BankAccount(2, "Piet", 500);
            
            // Act/Assert
            var exception = Assert.Throws<BankException>(() => sut.Transfer(destinationBankAccount, -500));
            
            Assert.Equal("Amount cannot be negative.", exception.Message);
        }      
        
        [Fact]
        public void Transfer_SourceBalanceInsufficient()
        {
            // Arrange
            var sut = new BankAccount(1, "Jan", 1000, -500);
            var destinationBankAccount = new BankAccount(2, "Piet", 500);
            
            // Act/Assert
            var exception = Assert.Throws<BankException>(() => sut.Transfer(destinationBankAccount, 1501));
            
            Assert.Equal("Balance insufficient.", exception.Message);
        }        
    }
}