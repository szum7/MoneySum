using ExcelWorkerApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelWorkerApp.Components.ReadExcel
{
    class TransactionGroupBuilder
    {
        public Dictionary<string, ExcelTransactionExtended> GroupDict;

        public TransactionGroupBuilder()
        {
            this.GroupDict = new Dictionary<string, ExcelTransactionExtended>();
        }

        public void AddPastDatedTransactions(ExcelSheet<ExcelTransactionExtended> excelSheet, DateTime accountDate)
        {
            var removeableKey = new List<string>();
            foreach (var keyValue in this.GroupDict)
            {
                if (keyValue.Value.AccountingDate < accountDate)
                {
                    excelSheet.AddNewRow(keyValue.Value);
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
                var groupedTransaction = this.GroupDict[groupId];
                this.SetGroupedTransaction(groupId, currentTransaction, groupedTransaction);
            }
            else // GroupId is new
            {
                var newGroupedTransaction = this.GetNewGroupedTransaction(groupId, currentTransaction, endDate);
                this.GroupDict.Add(groupId, newGroupedTransaction);
            }
        }

        void SetGroupedTransaction(string groupId, ExcelTransactionExtended currentTransaction, ExcelTransactionExtended groupedTransaction)
        {
            groupedTransaction.Sum += currentTransaction.Sum;
            groupedTransaction.GroupId = groupId;
            groupedTransaction.TagGroupId = currentTransaction.TagGroupId; // need to set it, since it may not already be set
            groupedTransaction.TagNames = currentTransaction.TagNames; // need to set it, since it may not already be set
        }

        ExcelTransactionExtended GetNewGroupedTransaction(string groupId, ExcelTransactionExtended currentTransaction, DateTime endDate)
        {
            return new ExcelTransactionExtended()
            {
                AccountingDate = endDate,
                Sum = currentTransaction.Sum,
                Currency = currentTransaction.Currency,
                GroupId = groupId,
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
