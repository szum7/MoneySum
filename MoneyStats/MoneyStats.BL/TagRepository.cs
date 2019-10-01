using MoneyStats.DAL;
using MoneyStats.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MoneyStats.BL
{
    public class TagRepository
    {
        public List<TagModel> Get()
        {
            using (var context = new DBContext())
            {
                return (from d in context.Tag
                        select new TagModel()
                        {
                            Id = d.Id,
                            Title = d.Title
                        }).ToList();
            }
        }

        public Dictionary<string, int> GetTitleKeyedDictionary()
        {
            using (var context = new DBContext())
            {
                return (from d in context.Tag
                        select new { d.Title, d.Id }).ToDictionary(k => k.Title, v => v.Id);
            }
        }

        public void Save(List<TagModel> tags)
        {
            using (var context = new DBContext())
            {
                context.Tag.AddRange(tags);
                context.SaveChanges();
            }
        }
    }
}
