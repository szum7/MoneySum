using MoneyStats.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using MoneyStats.DAL.Models;
using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Utility;
using MoneyStats.BL.Model.TagRepository;

namespace MoneyStats.BL
{
    public class TagRepository
    {
        public List<Tag> Get()
        {
            using (var context = new MoneyStatsContext())
            {
                return (from d in context.Tag
                        select new Tag()
                        {
                            Id = d.Id,
                            Title = d.Title
                        }).ToList();
            }
        }

        public Dictionary<string, int> GetTitleKeyedDictionary()
        {
            using (var context = new MoneyStatsContext())
            {
                return (from d in context.Tag
                        select new { d.Title, d.Id }).ToDictionary(k => k.Title, v => v.Id);
            }
        }

        public Dictionary<int, Tag> GetIdKeyedDictionary()
        {
            using (var context = new MoneyStatsContext())
            {
                return (from d in context.Tag
                        select new { d, d.Id }).ToDictionary(k => k.Id, v => v.d);
            }
        }

        public void Save(List<Tag> tags)
        {
            using (var context = new MoneyStatsContext())
            {
                context.Tag.AddRange(tags);
                context.SaveChanges();
            }
        }

        Dictionary<DateTime, MonthSummary> GetMonthsDictionary(List<DateTime> months)
        {
            var monthsDict = new Dictionary<DateTime, MonthSummary>();
            foreach (var month in months)
            {
                monthsDict.Add(month, new MonthSummary()
                {
                    Month = month,
                    TagInstances = new List<TagInstance>(),
                    Tags = new List<TagMonthSummaryLine>()
                });
            }
            return monthsDict;
        }

        DateTime AdjustFromDate(List<TransactionTagConn> transactionTagConns, DateTime from)
        {
            return new DateTime(Math.Max(transactionTagConns.First().Transaction.AccountingDate.Ticks, from.Ticks));
        }

        DateTime AdjustToDate(List<TransactionTagConn> transactionTagConns, DateTime to)
        {
            return new DateTime(Math.Min(transactionTagConns.Last().Transaction.AccountingDate.Ticks, to.Ticks));
        }

        public Dictionary<DateTime, MonthSummary> GetAllTagDetailedSummary(DateTime from, DateTime to)
        {
            var transTagConnRepo = new TransactionTagConnRepository();
            var transactionTagConns = transTagConnRepo.GetOnFilter(ttc => ttc.Transaction != null && ttc.Transaction.AccountingDate >= from && ttc.Transaction.AccountingDate <= to);
            //transactionTagConns.OrderByDescending(ttc => ttc.Transaction.AccountingDate);

            if (transactionTagConns.Count == 0)
            {
                return null;
            }

            from = this.AdjustFromDate(transactionTagConns, from);
            to = this.AdjustToDate(transactionTagConns, to);

            var months = Common.GetMonthsList(from, to);
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

        public void DeleteAll()
        {
            using (var context = new MoneyStatsContext())
            {
                context.Database.ExecuteSqlCommand("delete from dbo.[Tag];DBCC CHECKIDENT ([Tag], RESEED, 0);");
                context.SaveChanges();
            }
        }
    }
}
