using System;
using System.Linq;
using System.Collections.Generic;

namespace ExcelWorkerApp.Model
{
    public class ExcelSheet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Header { get; private set; }
        public List<ExcelTransaction> Transactions { get; private set; }

        public bool IsHeaderEmpty => this.Header.Count == 0;
        public int TransactionCount => this.Transactions.Count;

        public ExcelSheet()
        {
            this.Header = new List<string>();
            this.Transactions = new List<ExcelTransaction>();
        }

        public void OrderByDate()
        {
            this.Transactions.Sort((x, y) => x.AccountingDate.CompareTo(y.AccountingDate));
        }

        public void AddToHeader(string columnName)
        {
            this.Header.Add(columnName);
        }

        public void AddNewRow()
        {
            this.Transactions.Add(new ExcelTransaction());
        }

        public void AddNewRow(ExcelTransaction row)
        {
            this.Transactions.Add(row);
        }

        public void SetLastRow(ExcelTransaction value)
        {
            if (this.Transactions.Count == 0)
                return;
            this.Transactions[this.Transactions.Count - 1] = value;
        }

        public ExcelTransaction GetLastRow()
        {
            return this.Transactions.LastOrDefault();
        }

        public int Truncate()
        {
            this.OrderByDate();

            var tmp = new List<ExcelTransaction>();

            DateTime? maxDate = null;
            int truncatedRowCount = 0;
            foreach (var item in this.Transactions)
            {
                if (!maxDate.HasValue ||
                    item.AccountingDate >= maxDate.Value)
                {
                    var found = tmp.Any(t => t.IsTheSame(item));
                    if (found)
                    {
                        truncatedRowCount++;
                    }
                    else
                    {
                        tmp.Add(item);
                    }
                    maxDate = item.AccountingDate;
                }
                else
                {
                    truncatedRowCount++;
                }
            }

            this.Transactions = tmp;
            return truncatedRowCount;
        }

        public ExcelSheet RemoveOmittedRows()
        {
            var tmp = new List<ExcelTransaction>();
            foreach (var item in this.Transactions)
            {
                if (!item.IsOmitted)
                {
                    tmp.Add(item);
                }
            }

            Console.WriteLine($"Removed {this.Transactions.Count - tmp.Count} 'omitted' row(s).");

            this.Transactions = tmp;

            return this;
        }

        /// <summary>
        /// Assumes the first occurrence of the TagGroupId on a row has the list of tag names.
        /// </summary>
        public ExcelSheet ApplyTagsToTagGroups()
        {
            var groupDict = new Dictionary<string, List<string>>();
            foreach (var transaction in this.Transactions)
            {
                if (!String.IsNullOrWhiteSpace(transaction.TagGroupId))
                {
                    if (groupDict.ContainsKey(transaction.TagGroupId))
                    {
                        transaction.TagNames = groupDict[transaction.TagGroupId];
                    }
                    else if (transaction.TagNames.Count != 0)
                    {
                        groupDict.Add(transaction.TagGroupId, transaction.TagNames);
                    }
                }
            }
            return this;
        }

        public ExcelSheet ApplyGroups()
        {
            var tmp = new List<ExcelTransaction>();
            var groupBuilder = new TransactionGroupBuilder();
            foreach (var transaction in this.Transactions)
            {
                // AddPastDatedTransactions
                groupBuilder.AddPastDatedTransactions(tmp, transaction.AccountingDate);

                if (!String.IsNullOrWhiteSpace(transaction.GroupId))
                {
                    // StoreCurrentGroupedTransaction
                    groupBuilder.StoreCurrentGroupedTransaction(transaction, transaction.GroupId);
                }
                else
                {
                    tmp.Add(transaction);
                }
            }

            // Remaining end-of-dates group
            foreach (var keyValue in groupBuilder.GroupDict)
            {
                tmp.Add(keyValue.Value);
            }

            this.Transactions = tmp;
            return this;
        }

        public void MergeWith(ExcelSheet excelSheet2)
        {
            this.Transactions = this.Transactions.OrderBy(x => x.AccountingDate).ToList();
            var tr2 = excelSheet2.Transactions.OrderBy(x => x.AccountingDate).ToList();

            var merged = new List<ExcelTransaction>();
            int i = 0, j = 0;
            while (i < this.Transactions.Count && j < tr2.Count)
            {
                if (this.Transactions[i].AccountingDate < tr2[j].AccountingDate)
                {
                    merged.Add(this.Transactions[i]);
                    i++;
                }
                else if (this.Transactions[i].AccountingDate > tr2[j].AccountingDate)
                {
                    merged.Add(tr2[j]);
                    j++;
                }
                else
                {
                    // Transactions are not ordered on any other than dates
                    // therefor once dates are the same, we need to check
                    // if a transaction is unique on every iteration.
                    var interval = new List<ExcelTransaction>();
                    var date = this.Transactions[i].AccountingDate;
                    while (i < this.Transactions.Count && this.Transactions[i].AccountingDate == date)
                    {
                        if (!interval.Any(x => x.IsTheSame(this.Transactions[i])))
                        {
                            interval.Add(this.Transactions[i]);
                        }
                        i++;
                    }
                    while (j < tr2.Count && date == tr2[j].AccountingDate)
                    {
                        if (!interval.Any(x => x.IsTheSame(tr2[j])))
                        {
                            interval.Add(tr2[j]);
                        }
                        j++;
                    }
                    merged.AddRange(interval);
                }
            }
            while (i < this.Transactions.Count)
            {
                merged.Add(this.Transactions[i]);
                i++;
            }
            while (j < tr2.Count)
            {
                merged.Add(tr2[j]);
                j++;
            }

            this.Transactions = merged;
        }
    }
}
