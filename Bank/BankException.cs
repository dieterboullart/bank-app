using System;

namespace Bank
{
    public class BankException : Exception
    {
        public BankException(string message) : base((message))
        {
        }
    }
}