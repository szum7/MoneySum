using ExcelWorkerApp.Model;
using System;
using System.Collections.Generic;

namespace ExcelWorkerApp.Model
{
    class ExcelSheet<T> where T : ExcelTransaction, new()
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Header { get; set; }
        public List<T> Transactions { get; set; }

        public ExcelSheet()
        {
            this.Header = new List<string>();
            this.Transactions = new List<T>();
        }

        public bool IsHeaderEmpty() => this.Header.Count == 0;

        public void AddNewRow()
        {
            this.Transactions.Add(new T());
        }

        public void AddNewRow(T row)
        {
            this.Transactions.Add(row);
        }

        public void SetLastRow(T value)
        {
            if (this.Transactions.Count == 0)
                return;
            this.Transactions[this.Transactions.Count - 1] = value;
        }

        public T GetLastRow()
        {
            if (this.Transactions.Count == 0)
                return null;
            return this.Transactions[this.Transactions.Count - 1];
        }

        public int Truncate()
        {
            var tmp = new ExcelSheet<T>();

            DateTime? maxDate = null;
            string lastContentId = null;
            int truncatedRowCount = 0;
            foreach (var item in this.Transactions)
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

            this.Transactions = tmp.Transactions;
            return truncatedRowCount;
        }
    }
}
