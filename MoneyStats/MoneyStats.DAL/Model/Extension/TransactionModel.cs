using System;
using System.Collections.Generic;

namespace MoneyStats.DAL.Model
{
    public partial class TransactionModel
    {
        public ICollection<TagModel> Tags { get; set; }

        public TransactionModel()
        {
            this.TransactionTagConn = new List<TransactionTagConnModel>();
            this.Tags = new List<TagModel>();
        }

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
