using System;
using System.Collections.Generic;

namespace ExcelWorkerApp.Model
{
    public partial class ExcelTransaction
    {
        public int Id { get; set; }
        public DateTime AccountingDate { get; set; }
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public string PartnerAccount { get; set; }
        public string PartnerName { get; set; }
        public double Sum { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }

        public string ContentId
        {
            get
            {
                return $"{AccountingDate.ToString()}{TransactionId}{Type}{Account}{AccountName}{PartnerAccount}{PartnerName}{Sum.ToString("0.00")}{Currency}{Message}";
            }
        }

        public bool IsTheSame(ExcelTransaction other)
        {
            if (other == null)
                return false;
            return this.ContentId == other.ContentId;
        }
    }

    public partial class ExcelTransaction
    {
        public bool IsOmitted { get; set; }
        public string GroupId { get; set; }
        public List<string> TagNames { get; set; }
        public string TagGroupId { get; set; }

        public ExcelTransaction()
        {
            this.TagNames = new List<string>();
        }
    }
}
