using MoneyStats.DAL;
using System.Collections.Generic;
using System.Linq;
using MoneyStats.DAL.Models;

namespace MoneyStats.BL.TagRepo
{
    public class TagRepositoryLogic : TagRepositoryBase
    {
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
    }
}
