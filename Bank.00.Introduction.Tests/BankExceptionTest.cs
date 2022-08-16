using Xunit;

namespace Bank.Introduction.Tests
{
    public class BankExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            // Act
            var sut = new BankException("text");
            
            // Assert
            Assert.Equal("text", sut.Message);
        }
    }
}