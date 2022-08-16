using System.Collections.Generic;
using System.Linq;
using Bank.Implementations;
using Bank.Introduction.Tests.Fakes;
using Bank.Models;
using Xunit;

namespace Bank.Introduction.Tests.Implementations
{
    public class BankServiceTest
    {
        private readonly FakeBankRepository _bankRepository;
        private readonly FakeLogger _logger;

        private readonly BankService _sut;
        
        public BankServiceTest()
        {
            _bankRepository = new FakeBankRepository();
            _logger = new FakeLogger();

            _sut = new BankService(_bankRepository, _logger);
        }

        [Fact]
        public void GetAllAccounts()
        {
            // Arrange
            var bankAccountJan = new BankAccount(2, "Jan");
            var bankAccountPiet = new BankAccount(1, "Piet");
            
            _bankRepository.SetupGetAll(new List<BankAccount> { bankAccountJan, bankAccountPiet });

            // Act
            var result = _sut.GetAllAccounts();
            
            // Assert
            Assert.Equal(new[] { bankAccountPiet, bankAccountJan}, result);
        }

        [Fact]
        public void Deposit()
        {
            // Arrange
            var bankAccount = new BankAccount(1, "Jan", 100);
            _bankRepository.SetupGet(1, bankAccount);
            
            // Act
            var result = _sut.Deposit(1, 50);
            
            // Assert
            Assert.True(result);
            Assert.Equal(150, bankAccount.Balance);
            Assert.Contains(bankAccount, _bankRepository.SavedBankAccounts);
            Assert.Equal("Deposited 50 EUR to account 1.", _logger.SuccessMessage);
        }
        
        [Fact]
        public void Deposit_BankAccountNotFound()
        {
            // Act
            var result = _sut.Deposit(1, 50);
            
            // Assert
            Assert.False(result);
            Assert.Equal("Bank account 1 not found.", _logger.FailureMessage);
        }        
        
        [Fact]
        public void Deposit_ExceptionInSave()
        {
            // Arrange
            var bankAccount = new BankAccount(1, "Jan", 100);
            _bankRepository.SetupGet(1, bankAccount);
            _bankRepository.SetupSaveThrowsBankException("exception message");
            
            // Act
            var result = _sut.Deposit(1, 50);
            
            // Assert
            Assert.False(result);
            Assert.Equal("exception message", _logger.FailureMessage);
        }
        
        [Fact]
        public void Withdraw()
        {
            // Arrange
            var bankAccount = new BankAccount(1, "Jan", 100);
            _bankRepository.SetupGet(1, bankAccount);
            
            // Act
            var result = _sut.Withdraw(1, 50);
            
            // Assert
            Assert.True(result);
            Assert.Contains(bankAccount, _bankRepository.SavedBankAccounts);
            Assert.Equal("Withdrew 50 EUR from account 1.", _logger.SuccessMessage);
        }
        
        [Fact]
        public void Withdraw_BankAccountNotFound()
        {
            // Act
            var result = _sut.Withdraw(1, 50);
            
            // Assert
            Assert.False(result);
            Assert.Equal("Bank account 1 not found.", _logger.FailureMessage);
        }           
        
        [Fact]
        public void Withdraw_ExceptionInSave()
        {
            // Arrange
            var bankAccount = new BankAccount(1, "Jan", 100);
            _bankRepository.SetupGet(1, bankAccount);
            _bankRepository.SetupSaveThrowsBankException("exception message");
            
            // Act
            var result = _sut.Withdraw(1, 50);
            
            // Assert
            Assert.False(result);
            Assert.Equal("exception message", _logger.FailureMessage);
        }   
        
        [Fact]
        public void Transfer()
        {
            // Arrange
            var sourceBankAccount = new BankAccount(1, "Jan", 300);
            var destinationBankAccount = new BankAccount(2, "Piet",100);
            _bankRepository.SetupGet(1, sourceBankAccount);
            _bankRepository.SetupGet(2, destinationBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            Assert.True(result);
            Assert.Equal(250, sourceBankAccount.Balance);
            Assert.Equal(150, destinationBankAccount.Balance);
            Assert.Contains(sourceBankAccount, _bankRepository.SavedBankAccounts);
            Assert.Contains(destinationBankAccount, _bankRepository.SavedBankAccounts);
            Assert.Equal("Transferred 50 EUR from account 1 to account 2.", _logger.SuccessMessage);
        }      
        
        [Fact]
        public void Transfer_SourceBankAccountNotFound()
        {
            // Arrange
            var destinationBankAccount = new BankAccount(2, "Piet",100);
            _bankRepository.SetupGet(2, destinationBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            Assert.False(result);
            Assert.Equal(100, destinationBankAccount.Balance);
            Assert.DoesNotContain(destinationBankAccount, _bankRepository.SavedBankAccounts);
            Assert.Equal("Bank account 1 not found.", _logger.FailureMessage);
        }             
        
        [Fact]
        public void Transfer_DestinationBankAccountNotFound()
        {
            // Arrange
            var sourceBankAccount = new BankAccount(1, "Jan", 300);
            _bankRepository.SetupGet(1, sourceBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            Assert.False(result);
            Assert.Equal(300, sourceBankAccount.Balance);
            Assert.DoesNotContain(sourceBankAccount, _bankRepository.SavedBankAccounts);
            Assert.Equal("Bank account 2 not found.", _logger.FailureMessage);
        }        
        
        [Fact]
        public void Transfer_ExceptionInSave()
        {
            // Arrange
            var sourceBankAccount = new BankAccount(1, "Jan", 300);
            var destinationBankAccount = new BankAccount(2, "Piet",100);
            _bankRepository.SetupGet(1, sourceBankAccount);
            _bankRepository.SetupGet(2, destinationBankAccount);
            _bankRepository.SetupSaveThrowsBankException("exception message");

            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            Assert.False(result);
            Assert.DoesNotContain(destinationBankAccount, _bankRepository.SavedBankAccounts);
            Assert.Equal("exception message", _logger.FailureMessage);
        }            
    }
}