using ExcelWorkerApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelWorkerApp.Components.MergeTransaction
{
    /// <summary>
    /// 
    /// </summary>
    class TransactionMerger
    {
        public List<Transaction> Run(List<Transaction> tr1, List<Transaction> tr2)
        {
            List<Transaction> merged = new List<Transaction>();

            // Sort tr1 on date
            // Sort tr2 on date
            // Merge

            return merged;
        }
    }
}
