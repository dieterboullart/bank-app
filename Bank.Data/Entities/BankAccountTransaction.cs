using System;

namespace Bank.Data.Entities
{
    public abstract class BankAccountTransaction
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        
        public int AccountId { get; set; }

        #region Navigation properties

        public BankAccount Account { get; set; }

        #endregion
    }
}