using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelWorkerApp.Model
{
    public class TransactionExtended : Transaction
    {
        public bool IsOmitted { get; set; }
        public string GroupId { get; set; }
        public List<string> TagNames { get; set; }
        public string TagGroupId { get; set; }

        public TransactionExtended()
        {
            this.TagNames = new List<string>();
        }
    }
}
