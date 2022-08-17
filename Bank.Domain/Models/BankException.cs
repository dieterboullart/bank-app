using System;

namespace Bank.Domain.Models
{
    public class BankException : Exception
    {
        public BankException(string message) : base((message))
        {
        }
    }
}