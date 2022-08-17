using Bank.BL.Services;
using Bank.Data.Repositories.Interfaces;
using Bank.Domain.Models;
using Bank.Shared.Logging.Interfaces;
using Bank.Shared.Utils.Clock.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Bank.BL.Tests.Services
{
    public class BankServiceTest
    {
        private readonly Person _holder = new("Jan", "Janssen");
        private readonly Person _otherHolder = new("Peter", "Peterssen");
        
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ILogger _logger;

        private readonly BankService _sut;
 
        private readonly DateTime _transactionDate = new(2021, 8, 5)

            ;        public BankServiceTest()
        {
            _bankAccountRepository = A.Fake<IBankAccountRepository>();
            _logger = A.Fake<ILogger>();

            var clock = A.Fake<IClock>();

            A.CallTo(() => clock.CurrentUtcTime)
                .Returns(_transactionDate);

            A.CallTo(() => _bankAccountRepository.Get(A<int>._))
                .Returns(null); // without this the _bankRepository.Get( will return a fake BankAccount
            
            _sut = new BankService(_bankAccountRepository, _logger, clock);
        }

        [Fact]
        public void GetAllAccounts()
        {
            // Arrange
            var bankAccountJan = new BankAccountSummary(2, 200, "Jan Janssen", 0, 0);
            var bankAccountPiet = new BankAccountSummary(1, 101, "Peter Peterssen", 0, 0);
            
            A.CallTo(() => _bankAccountRepository.GetSummaries())
                .Returns(new List<BankAccountSummary> { bankAccountJan, bankAccountPiet });

            // Act
            var result = _sut.GetSummaries();
            
            // Assert
            result.Should().Equal(bankAccountPiet, bankAccountJan);
        }
        
        [Fact]
        public void Deposit()
        {
            // Arrange
            var bankAccount = CreateBankAccount(1, _holder, 100);

            A.CallTo(() => _bankAccountRepository.Get(1))
                .Returns(bankAccount);
            
            // Act
            var result = _sut.Deposit(1, 50);
            
            // Assert
            result.Should().Be(true);
            bankAccount.Balance.Should().Be(150);
            A.CallTo(() => _bankAccountRepository.Save(bankAccount))
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
            var bankAccount = CreateBankAccount(1, _holder, 100);
            A.CallTo(() => _bankAccountRepository.Get(1))
                .Returns(bankAccount);
            A.CallTo(() => _bankAccountRepository.Save(bankAccount))
                .Throws(new BankException("exception message"));
            
            // Act
            var result = _sut.Deposit(1, 50);
            
            // Assert
            result.Should().Be(false);
            //bankAccount.Balance.Should().Be(100);
            A.CallTo(() => _logger.LogFailure("exception message"))
                .MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Withdraw()
        {
            // Arrange
            var bankAccount = CreateBankAccount(1, _holder, 100);
            A.CallTo(() => _bankAccountRepository.Get(1))
                .Returns(bankAccount);
            
            // Act
            var result = _sut.Withdraw(1, 50);
            
            // Assert
            result.Should().Be(true);
            //bankAccount.Balance.Should().Be(50);
            A.CallTo(() => _bankAccountRepository.Save(bankAccount))
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
            var bankAccount = CreateBankAccount(1, _holder, 100);
            A.CallTo(() => _bankAccountRepository.Get(1))
                .Returns(bankAccount);
            A.CallTo(() => _bankAccountRepository.Save(bankAccount))
                .Throws(new BankException("exception message"));
            
            // Act
            var result = _sut.Withdraw(1, 50);
            
            // Assert
            result.Should().Be(false);
            //bankAccount.Balance.Should().Be(100);
            A.CallTo(() => _logger.LogFailure("exception message"))
                .MustHaveHappenedOnceExactly();
        }   
        
        [Fact]
        public void Transfer()
        {
            // Arrange
            var sourceBankAccount = CreateBankAccount(1, _holder, 100);
            var destinationBankAccount = CreateBankAccount(2, _otherHolder, 100);
            A.CallTo(() => _bankAccountRepository.Get(1))
                .Returns(sourceBankAccount);
            A.CallTo(() => _bankAccountRepository.Get(2))
                .Returns(destinationBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            result.Should().Be(true);
            sourceBankAccount.Balance.Should().Be(50);
            destinationBankAccount.Balance.Should().Be(150);
            A.CallTo(() => _bankAccountRepository.Save(sourceBankAccount))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _bankAccountRepository.Save(destinationBankAccount))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _logger.LogSuccess("Transferred 50 EUR from account 1 to account 2."))
                .MustHaveHappenedOnceExactly();
        }      
        
        [Fact]
        public void Transfer_SourceBankAccountNotFound()
        {
            // Arrange
            var destinationBankAccount = CreateBankAccount(2, _otherHolder, 100);
            A.CallTo(() => _bankAccountRepository.Get(2))
                .Returns(destinationBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            result.Should().Be(false);
            destinationBankAccount.Balance.Should().Be(100);
            A.CallTo(() => _bankAccountRepository.Save(destinationBankAccount))
                .MustNotHaveHappened();
            A.CallTo(() => _logger.LogFailure("Bank account 1 not found."))
                .MustHaveHappenedOnceExactly();
        }             
        
        [Fact]
        public void Transfer_DestinationBankAccountNotFound()
        {
            // Arrange
            var sourceBankAccount = CreateBankAccount(1, _holder, 100);
            A.CallTo(() => _bankAccountRepository.Get(1))
                .Returns(sourceBankAccount);
            
            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            result.Should().Be(false);
            sourceBankAccount.Balance.Should().Be(100);
            A.CallTo(() => _bankAccountRepository.Save(sourceBankAccount))
                .MustNotHaveHappened();
            A.CallTo(() => _logger.LogFailure("Bank account 2 not found."))
                .MustHaveHappenedOnceExactly();
        }        
        
        [Fact]
        public void Transfer_ExceptionInSave()
        {
            // Arrange
            var sourceBankAccount = CreateBankAccount(1, _holder, 100);
            var destinationBankAccount = CreateBankAccount(2, _otherHolder, 100);
            A.CallTo(() => _bankAccountRepository.Get(1))
                .Returns(sourceBankAccount);
            A.CallTo(() => _bankAccountRepository.Get(2))
                .Returns(destinationBankAccount);
            A.CallTo(() => _bankAccountRepository.Save(sourceBankAccount))
                .Throws(new BankException("exception message"));

            // Act
            var result = _sut.Transfer(1, 2, 50);
            
            // Assert
            result.Should().Be(false);
            //sourceBankAccount.Balance.Should().Be(300);
            //destinationBankAccount.Balance.Should().Be(100);
            A.CallTo(() => _bankAccountRepository.Save(destinationBankAccount))
                .MustNotHaveHappened();
            A.CallTo(() => _logger.LogFailure("exception message"))
                .MustHaveHappenedOnceExactly();
        }      
        
        private static BankAccount CreateBankAccount(int id, Person holder, int balance = 0)
        {
            return new(id, holder, balance, 0, null);
        }         
    }
}