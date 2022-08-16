using System.Collections.Generic;
using Bank.Implementations;
using Bank.Interfaces;
using Bank.Models;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Bank.FakeItEasy.Tests.Implementations
{
    public class BankServiceTest
    {
        private readonly IBankRepository _bankRepository;
        private readonly ILogger _logger;

        private readonly BankService _sut;
        
        public BankServiceTest()
        {
            _bankRepository = A.Fake<IBankRepository>();
            _logger = A.Fake<ILogger>();

            A.CallTo(() => _bankRepository.Get(A<int>._))
                .Returns(null); // without this the _bankRepository.Get( will return a fake BankAccount
            
            _sut = new BankService(_bankRepository, _logger);
        }

        [Fact]
        public void GetAllAccounts()
        {
            // Arrange
            var bankAccountJan = new BankAccount(2, "Jan");
            var bankAccountPiet = new BankAccount(1, "Piet");
            
            A.CallTo(() => _bankRepository.GetAll())
                .Returns(new List<BankAccount> { bankAccountJan, bankAccountPiet });

            // Act
            var result = _sut.GetAllAccounts();
            
            // Assert
            result.Should().Equal(bankAccountPiet, bankAccountJan);
        }

        [Fact]
        public void Deposit()
        {
            // Arrange
            var bankAccount = new BankAccount(1, "Jan", 100);

            A.CallTo(() => _bankRepository.Get(1))
                .Returns(bankAccount);
            
            // Act
            var result = _sut.Deposit(1, 50);
            
            // Assert
            result.Should().Be(true);
            bankAccount.Balance.Should().Be(150);
            A.CallTo(() => _bankRepository.Save(bankAccount))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _logger.LogSuccess("Deposited 50 EUR to account 1."))
                .MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Deposit_BankAccountNotFound()
        {
            // Act
            var result = _sut.Deposit(1, 50);
            
            // Assert
            Assert.False(result);
            A.CallTo(() => _logger.LogFailure("Bank account 1 not found."))
                .MustHaveHappenedOnceExactly();
        }        
        
        [Fact]
        public void Deposit_ExceptionInSave()
        {
            // Arrange
            var bankAccount = new BankAccount(1, "Jan", 100);
            A.CallTo(() => _bankRepository.Get(1))
                .Returns(bankAccount);
            A.CallTo(() => _bankRepository.Save(bankAccount))
                .Throws(new BankException("exception message"));
            
            // Act
            var result = _sut.Deposit(1, 50);
            
            // Assert
            result.Should().Be(false);
            A.CallTo(() => _logger.LogFailure("exception message"))
                .MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Withdraw()
        {
            // Arrange
            var bankAccount = new BankAccount(1, "Jan", 100);
            A.CallTo(() => _bankRepository.Get(1))
                .Returns(bankAccount);
            
            // Act
            var result = _sut.Withdraw(1, 50);
            
            // Assert
            result.Should().Be(true);
            A.CallTo(() => _bankRepository.Save(bankAccount))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _logger.LogSuccess("Withdrew 50 EUR from account 1."))
                .MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Withdraw_BankAccountNotFound()
        {
            // Act
            var result = _sut.Withdraw(1, 50);
            
            // Assert
            result.Should().Be(false);
            A.CallTo(() => _logger.LogFailure("Bank account 1 not found."))
                .MustHaveHappenedOnceExactly();
        }           
        
        [Fact]
        public void Withdraw_ExceptionInSave()
        {
            // Arrange
            var bankAccount = new BankAccount(1, "Jan", 100);
            A.CallTo(() => _bankRepository.Get(1))
                .Returns(bankAccount);
            A.CallTo(() => _bankRepository.Save(bankAccount))
                .Throws(new BankException("exception message"));
            
            // Act
            var result = _sut.Withdraw(1, 50);
            
            // Assert
            result.Should().Be(false);
            A.CallTo(() => _logger.LogFailure("exception message"))
                .MustHaveHappenedOnceExactly();
        }   
        
        [Fact]
        public void Transfer()
        {
            // Arrange
            var sourceBankAccount = new BankAccount(1, "Jan", 300);
            var destinationBankAccount = new BankAccount(2, "Piet",100);
            A.CallTo(() => _bankRepository.Get(1))
                .Returns(sourceBankAccount);
            A.CallTo(() => _bankRepository.Get(2))
                .Returns(destinationBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            result.Should().Be(true);
            sourceBankAccount.Balance.Should().Be(250);
            destinationBankAccount.Balance.Should().Be(150);
            A.CallTo(() => _bankRepository.Save(sourceBankAccount))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _bankRepository.Save(destinationBankAccount))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _logger.LogSuccess("Transferred 50 EUR from account 1 to account 2."))
                .MustHaveHappenedOnceExactly();
        }      
        
        [Fact]
        public void Transfer_SourceBankAccountNotFound()
        {
            // Arrange
            var destinationBankAccount = new BankAccount(2, "Piet",100);
            A.CallTo(() => _bankRepository.Get(2))
                .Returns(destinationBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            result.Should().Be(false);
            destinationBankAccount.Balance.Should().Be(100);
            A.CallTo(() => _bankRepository.Save(destinationBankAccount))
                .MustNotHaveHappened();
            A.CallTo(() => _logger.LogFailure("Bank account 1 not found."))
                .MustHaveHappenedOnceExactly();
        }             
        
        [Fact]
        public void Transfer_DestinationBankAccountNotFound()
        {
            // Arrange
            var sourceBankAccount = new BankAccount(1, "Jan", 300);
            A.CallTo(() => _bankRepository.Get(1))
                .Returns(sourceBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            result.Should().Be(false);
            sourceBankAccount.Balance.Should().Be(300);
            A.CallTo(() => _bankRepository.Save(sourceBankAccount))
                .MustNotHaveHappened();
            A.CallTo(() => _logger.LogFailure("Bank account 2 not found."))
                .MustHaveHappenedOnceExactly();
        }        
        
        [Fact]
        public void Transfer_ExceptionInSave()
        {
            // Arrange
            var sourceBankAccount = new BankAccount(1, "Jan", 300);
            var destinationBankAccount = new BankAccount(2, "Piet",100);
            A.CallTo(() => _bankRepository.Get(1))
                .Returns(sourceBankAccount);
            A.CallTo(() => _bankRepository.Get(2))
                .Returns(destinationBankAccount);
            A.CallTo(() => _bankRepository.Save(sourceBankAccount))
                .Throws(new BankException("exception message"));

            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            result.Should().Be(false);
            A.CallTo(() => _bankRepository.Save(destinationBankAccount))
                .MustNotHaveHappened();
            A.CallTo(() => _logger.LogFailure("exception message"))
                .MustHaveHappenedOnceExactly();
        }            
    }
}