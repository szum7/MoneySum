using ExcelWorkerApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelWorkerApp.Model
{
    class TransactionGroupBuilder
    {
        public Dictionary<string, ExcelTransactionExtended> GroupDict;

        public TransactionGroupBuilder()
        {
            this.GroupDict = new Dictionary<string, ExcelTransactionExtended>();
        }

        public void AddPastDatedTransactions(List<ExcelTransactionExtended> transactions, DateTime accountDate)
        {
            var removeableKey = new List<string>();
            foreach (var keyValue in this.GroupDict)
            {
                if (keyValue.Value.AccountingDate < accountDate)
                {
                    transactions.Add(keyValue.Value);
                    removeableKey.Add(keyValue.Key);
                }
            }
            foreach (var keyValue in removeableKey)
            {
                this.GroupDict.Remove(keyValue);
            }
        }

        public void StoreCurrentGroupedTransaction(ExcelTransactionExtended currentTransaction, string groupId)
        {
            var endDate = this.GetEndDayDate(currentTransaction.AccountingDate);

            if (this.GroupDict.ContainsKey(groupId)) // GroupId already exists
            {
                this.GroupDict[groupId].Sum += currentTransaction.Sum;
            }
            else // GroupId is new
            {
                var newGroupedTransaction = this.GetNewGroupedTransaction(currentTransaction, endDate);
                this.GroupDict.Add(groupId, newGroupedTransaction);
            }
        }

        ExcelTransactionExtended GetNewGroupedTransaction(ExcelTransactionExtended currentTransaction, DateTime endDate)
        {
            return new ExcelTransactionExtended()
            {
                AccountingDate = endDate,
                Sum = currentTransaction.Sum,
                Currency = currentTransaction.Currency,
                GroupId = currentTransaction.GroupId,
                TagNames = currentTransaction.TagNames,
                TagGroupId = currentTransaction.TagGroupId
            };
        }

        DateTime GetEndDayDate(DateTime date)
        {
            return (new DateTime(date.Year, date.Month + 1, 1)).AddDays(-1);
        }
    }
}
