using System.Collections.Generic;

namespace Bank.Data.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public decimal MinBalance { get; set; }
        public int HolderId { get; set; }

        #region Navigation properties
        
        public Person Holder { get; set; }
        public IList<BankAccountTransaction> Transactions { get; set; }

        #endregion
    }
}