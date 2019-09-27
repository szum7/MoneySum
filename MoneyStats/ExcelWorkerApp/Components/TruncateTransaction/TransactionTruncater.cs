using ExcelWorkerApp.Components.ReadExcel;
using ExcelWorkerApp.Model;
using ExcelWorkerApp.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelWorkerApp.Components.TruncateTransaction
{
    class TransactionTruncater<T> where T: Transaction, new()
    {
        ConsoleWatch watch;

        public TransactionTruncater()
        {
            this.watch = new ConsoleWatch(this.GetType().Name);
        }

        /// <summary>
        /// original.Transactions must be in asc order (with intersections)
        /// </summary>
        /// <param name="original"></param>
        public void Run(ExcelSheet<T> original)
        {
            var tmp = new ExcelSheet<T>();

            DateTime? maxDate = null;
            string lastContentId = null;
            int truncatedRowCount = 0;
            foreach (var item in original.Transactions)
            {
                if ((!maxDate.HasValue) || 
                    (item.AccountingDate > maxDate) || 
                    (item.AccountingDate == maxDate && (lastContentId == null || lastContentId != item.ContentId)))
                {
                    tmp.AddNewRow(item);

                    lastContentId = item.ContentId;
                    maxDate = item.AccountingDate;
                }
                else
                {
                    truncatedRowCount++;
                }
            }

            original.Transactions = tmp.Transactions;

            this.watch.PrintDiff($"FINISHED. {truncatedRowCount} truncated rows, {tmp.Transactions.Count} remaining.");
        }
    }
}
