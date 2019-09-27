using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelWorkerApp.Model
{
    public class TransactionExtended : Transaction
    {
        public bool IsOmitted { get; set; }
        public string GroupId { get; set; }
        public List<int> TagIds { get; set; }

        public TransactionExtended()
        {
            this.TagIds = new List<int>();
        }
    }
}
