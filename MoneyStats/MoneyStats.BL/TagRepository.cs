using MoneyStats.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using MoneyStats.DAL.Models;
using Microsoft.EntityFrameworkCore;

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

        public void Save(List<Tag> tags)
        {
            using (var context = new MoneyStatsContext())
            {
                context.Tag.AddRange(tags);
                context.SaveChanges();
            }
        }
    }
}
