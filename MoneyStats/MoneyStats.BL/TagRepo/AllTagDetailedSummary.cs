using System;
using System.Collections.Generic;
using System.Linq;
using MoneyStats.DAL.Models;
using MoneyStats.BL.Utility;
using MoneyStats.BL.TagRepo.Model;

namespace MoneyStats.BL.TagRepo
{
    public class AllTagDetailedSummary
    {
        public Dictionary<DateTime, TagMonthSummary> Get(DateTime startDate, DateTime endDate)
        {
            var transTagConnRepo = new TransactionTagConnRepository();
            var transactionTagConns = transTagConnRepo.GetOnFilter(ttc => ttc.Transaction != null && ttc.Transaction.AccountingDate >= startDate && ttc.Transaction.AccountingDate <= endDate);
            //transactionTagConns.OrderByDescending(ttc => ttc.Transaction.AccountingDate);

            if (transactionTagConns.Count == 0)
            {
                return null;
            }

            startDate = this.AdjustStartDate(transactionTagConns, startDate);
            endDate = this.AdjustEndDate(transactionTagConns, endDate);

            var months = Common.GetMonthsList(startDate, endDate);
            var monthsDict = this.GetMonthsDictionary(months);

            foreach (var transactionTagConn in transactionTagConns)
            {
                var currentTransaction = transactionTagConn.Transaction;
                var currentMonth = new DateTime(currentTransaction.AccountingDate.Year, currentTransaction.AccountingDate.Month, 1);

                if (!monthsDict.ContainsKey(currentMonth))
                {
                    // Transaction is out of the given date range!
                    continue;
                }

                var monthDictRow = monthsDict[currentMonth];
                var tag = monthDictRow.Tags.SingleOrDefault(tag => tag.Title == transactionTagConn.Tag.Title);

                if (tag == null)
                {
                    monthDictRow.Tags.Add(new TagMonthSummaryLine()
                    {
                        Month = currentMonth,
                        Title = transactionTagConn.Tag.Title,
                        Income = (currentTransaction.Sum > 0 ? currentTransaction.Sum.Value : 0),
                        Expense = (currentTransaction.Sum < 0 ? currentTransaction.Sum.Value : 0),
                        Transactions = new List<Transaction>() { currentTransaction }
                    });
                }
                else
                {
                    tag.Income += (currentTransaction.Sum > 0 ? currentTransaction.Sum.Value : 0);
                    tag.Expense += (currentTransaction.Sum < 0 ? currentTransaction.Sum.Value : 0);
                    tag.Transactions.Add(currentTransaction);
                }
            }

            return monthsDict;
        }

        Dictionary<DateTime, TagMonthSummary> GetMonthsDictionary(List<DateTime> months)
        {
            var monthsDict = new Dictionary<DateTime, TagMonthSummary>();
            foreach (var month in months)
            {
                monthsDict.Add(month, new TagMonthSummary()
                {
                    Month = month,
                    TagInstances = new List<TagInstance>(),
                    Tags = new List<TagMonthSummaryLine>()
                });
            }
            return monthsDict;
        }

        DateTime AdjustStartDate(List<TransactionTagConn> transactionTagConns, DateTime from) =>
            new DateTime(Math.Max(transactionTagConns.First().Transaction.AccountingDate.Ticks, from.Ticks));

        DateTime AdjustEndDate(List<TransactionTagConn> transactionTagConns, DateTime to) =>
            new DateTime(Math.Min(transactionTagConns.Last().Transaction.AccountingDate.Ticks, to.Ticks));
    }
}
