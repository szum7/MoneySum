using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;

namespace MoneyStats.DAL.Model
{
    public partial class Transaction : DBModel
    {
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

        public Currency Currency { get; set; }
        public ICollection<TransactionTagConnModel> TransactionTagConn { get; set; }
    }
}
