using ExcelWorkerApp.Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ExcelWorkerApp.Model
{
    public class ExcelSheet<T> where T : ExcelTransaction, new()
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
            var tmp = new List<T>();

            DateTime? maxDate = null;
            int truncatedRowCount = 0;
            foreach (var item in this.Transactions)
            {
                if (!maxDate.HasValue ||
                    item.AccountingDate >= maxDate.Value)
                {
                    var found = tmp.Any(t => t.ContentId == item.ContentId);
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

        public ExcelSheet<T> RemoveOmittedRows()
        {
            if (!typeof(ExcelTransactionExtended).IsAssignableFrom(typeof(T)))
            {
                Console.WriteLine("ExcelSheet is not ExcelTransactionExtended!");
                return this;
            }

            var tmp = new List<ExcelTransactionExtended>();
            var castList = this.Transactions as List<ExcelTransactionExtended>;
            foreach (var item in castList)
            {
                if (!item.IsOmitted)
                {
                    tmp.Add(item);
                }
            }

            Console.WriteLine($"Removed {this.Transactions.Count - tmp.Count} row(s).");

            this.Transactions = tmp as List<T>;

            return this;
        }

        /// <summary>
        /// Assumes the first occurrence of the TagGroupId on a row has the list of tag names.
        /// </summary>
        public ExcelSheet<T> ApplyTagsToTagGroups()
        {
            if (!typeof(ExcelTransactionExtended).IsAssignableFrom(typeof(T)))
            {
                Console.WriteLine("ExcelSheet is not ExcelTransactionExtended!");
                return this;
            }

            var groupDict = new Dictionary<string, List<string>>();
            var castList = this.Transactions as List<ExcelTransactionExtended>;
            foreach (var transaction in castList)
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

        public ExcelSheet<T> ApplyGroups()
        {
            if (!typeof(ExcelTransactionExtended).IsAssignableFrom(typeof(T)))
            {
                Console.WriteLine("ExcelSheet is not ExcelTransactionExtended!");
                return this;
            }

            var tmp = new List<ExcelTransactionExtended>();
            var castList = this.Transactions as List<ExcelTransactionExtended>;
            var groupDict = new Dictionary<string, ExcelTransactionExtended>();
            foreach (var transaction in castList)
            {
                // AddPastDatedTransactions
                var removeableKey = new List<string>();
                foreach (var keyValue in groupDict)
                {
                    if (keyValue.Value.AccountingDate < transaction.AccountingDate)
                    {
                        tmp.Add(keyValue.Value);
                        removeableKey.Add(keyValue.Key);
                    }
                }
                foreach (var keyValue in removeableKey)
                {
                    groupDict.Remove(keyValue);
                }

                // StoreCurrentGroupedTransaction
                if (!String.IsNullOrWhiteSpace(transaction.GroupId))
                {
                    var endDate = this.GetEndDayDate(transaction.AccountingDate);
                    if (groupDict.ContainsKey(transaction.GroupId)) // GroupId already exists
                    {
                        var groupedTransaction = groupDict[transaction.GroupId];

                        // SetGroupedTransaction
                        groupedTransaction.Sum += transaction.Sum;
                    }
                    else // GroupId is new
                    {
                        var newGroupedTransaction = new ExcelTransactionExtended()
                        {
                            AccountingDate = endDate,
                            Sum = transaction.Sum,
                            Currency = transaction.Currency,
                            GroupId = transaction.GroupId,
                            TagNames = transaction.TagNames,
                            TagGroupId = transaction.TagGroupId
                        };
                        groupDict.Add(transaction.GroupId, newGroupedTransaction);
                    }
                }
                else
                {
                    tmp.Add(transaction);
                }
            }

            // Remaining end-of-dates group
            foreach (var keyValue in groupDict)
            {
                tmp.Add(keyValue.Value);
            }

            this.Transactions = tmp as List<T>;
            return this;
        }

        DateTime GetEndDayDate(DateTime date)
        {
            return (new DateTime(date.Year, date.Month + 1, 1)).AddDays(-1);
        }
    }
}
