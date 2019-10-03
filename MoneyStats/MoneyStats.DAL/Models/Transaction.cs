using System;
using System.Collections.Generic;

namespace MoneyStats.DAL.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public DateTime AccountingDate { get; set; }
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public string PartnerAccount { get; set; }
        public string PartnerName { get; set; }
        public decimal? Sum { get; set; }
        public int? CurrencyId { get; set; }
        public string Message { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string State { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual ICollection<TransactionTagConn> TransactionTagConn { get; set; }
    }
}
