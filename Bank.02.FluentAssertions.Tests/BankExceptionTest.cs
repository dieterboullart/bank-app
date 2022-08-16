using FluentAssertions;
using Xunit;

namespace Bank.FakeItEasy.Tests
{
    public class BankExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            // Act
            var sut = new BankException("text");
            
            // Assert
            sut.Message.Should().Be("text");
        }
    }
}