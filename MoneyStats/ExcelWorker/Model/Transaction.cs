using System;

namespace ExcelWorker.Model
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Number { get { return this.Id; } set { this.Id = value; } }

        public DateTime AccountingDate { get; set; }
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public string PartnerAccount { get; set; }
        public string PartnerName { get; set; }
        public decimal Sum { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }

        public string ContentId
        {
            get
            {
                return $"{AccountingDate.ToString()}{TransactionId}{Type}{Account}{AccountName}{PartnerAccount}{PartnerName}{Sum.ToString()}{Currency}{Message}";
            }
        }

        public bool IsTheSame(Transaction other)
        {
            if (other == null)
                return false;
            return this.ContentId == other.ContentId;
        }
    }
}
