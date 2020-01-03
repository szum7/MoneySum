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

        public void GetAllTagDetailedSummary(int tagId, DateTime from, DateTime to)
        {
            var dates = Common.GetDatesList(from, to);
            var transRepo = new TransactionRepository();
            var transTagConnRepo = new TransactionTagConnRepository();
            var tagRepo = new TagRepository();

            // create year-month dictionary
            // iterate transTagConnList and associate on date <-> year-month key
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
