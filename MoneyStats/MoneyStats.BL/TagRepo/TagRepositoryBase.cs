using MoneyStats.DAL;
using System.Collections.Generic;
using System.Linq;
using MoneyStats.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace MoneyStats.BL.TagRepo
{
    public class TagRepositoryBase
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

        public void Save(List<Tag> tags)
        {
            using (var context = new MoneyStatsContext())
            {
                context.Tag.AddRange(tags);
                context.SaveChanges();
            }
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
