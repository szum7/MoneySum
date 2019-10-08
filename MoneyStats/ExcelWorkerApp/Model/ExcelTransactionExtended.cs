using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelWorkerApp.Model
{
    public class ExcelTransactionExtended : ExcelTransaction
    {
        public string GroupId { get; set; }
        public List<string> TagNames { get; set; }
        public string TagGroupId { get; set; }

        public ExcelTransactionExtended()
        {
            this.TagNames = new List<string>();
        }
    }
}
