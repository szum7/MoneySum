using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyStats.DAL.Models
{
    public partial class Transaction
    {
        [NotMapped]
        public List<Tag> Tags { get; set; }

        public Transaction()
        {
            this.TransactionTagConn = new HashSet<TransactionTagConn>();
            this.Tags = new List<Tag>();
        }

        [NotMapped]
        public string ContentId
        {
            get
            {
                string sum = Sum.HasValue ? Sum.ToString() : "";
                string currency = Currency != null ? Currency.Name : "";
                return $"{AccountingDate.ToString()}{TransactionId}{Type}{Account}{AccountName}{PartnerAccount}{PartnerName}{sum}{currency}{Message}";
            }
        }
    }
}
