using MoneyStats.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using MoneyStats.DAL.Models;
using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.Utility;

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

        class TagMonthSummary
        {
            public DateTime Month { get; set; }
            public string Title { get; set; }
            public decimal Income { get; set; }
            public decimal Expense { get; set; }
            public decimal Flow { get { return Income - Math.Abs(Expense); } }
            public List<Transaction> Transactions { get; set; }
        }

        public void GetAllTagDetailedSummary(int tagId, DateTime from, DateTime to)
        {
            var months = Common.GetMonthsList(from, to);
            var monthsDict = new Dictionary<DateTime, TagMonthSummary>();
            var transRepo = new TransactionRepository();
            var transTagConnRepo = new TransactionTagConnRepository();
            var tagRepo = new TagRepository();

            var ttcs = transTagConnRepo.GetWithEntities();

            foreach (var ttc in ttcs)
            {
                var current = ttc.Transaction;
                var currentMonth = new DateTime(current.AccountingDate.Year, current.AccountingDate.Month, 1);
                if (!monthsDict.ContainsKey(currentMonth))
                {
                    monthsDict.Add(currentMonth, new TagMonthSummary()
                    {
                        Month = currentMonth,
                        Title = ttc.Tag.Title,
                        Income = (current.Sum > 0 ? current.Sum.Value : 0),
                        Expense = (current.Sum < 0 ? current.Sum.Value : 0),
                        Transactions = new List<Transaction>() { current }
                    });
                } 
                else
                {
                    var monthDictRow = monthsDict[currentMonth];
                    monthDictRow.Income += (current.Sum > 0 ? current.Sum.Value : 0);
                    monthDictRow.Expense += (current.Sum < 0 ? current.Sum.Value : 0);
                    monthDictRow.Transactions.Add(current);
                    // TODO ROSSZ!!! nem 1 tag van hónaponként! kijjebb vinni a dátumot!
                }
            }

            // create year-month dictionary
            // iterate transTagConnList and associate on date <-> year-month key
            // orderby date
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
