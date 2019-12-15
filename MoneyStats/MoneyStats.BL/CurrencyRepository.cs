using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MoneyStats.DAL;
using Microsoft.EntityFrameworkCore;

namespace MoneyStats.BL
{
    public class CurrencyRepository
    {
        public List<Currency> Get()
        {
            using (var context = new MoneyStatsContext())
            {
                return (from d in context.Currency
                        select new Currency()
                        {
                            Id = d.Id,
                            Name = d.Name
                        }).ToList();
            }
        }

        public Dictionary<string, int> GetTitleKeyedDictionary()
        {
            using (var context = new MoneyStatsContext())
            {
                return (from d in context.Currency
                        select new { d.Name, d.Id }).ToDictionary(k => k.Name, v => v.Id);
            }
        }

        public void Save(List<Currency> currencies)
        {
            using (var context = new MoneyStatsContext())
            {
                context.Currency.AddRange(currencies);
                context.SaveChanges();
            }
        }

        public void DeleteAll()
        {
            using (var context = new MoneyStatsContext())
            {
                context.Database.ExecuteSqlCommand("delete from dbo.[Currency];DBCC CHECKIDENT ([Currency], RESEED, 0);");
                context.SaveChanges();
            }
        }
    }
}
