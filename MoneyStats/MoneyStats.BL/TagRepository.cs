using MoneyStats.BL.TagRepo;
using MoneyStats.BL.TagRepo.Model;
using System;
using System.Collections.Generic;

namespace MoneyStats.BL
{
    public class TagRepository : TagRepositoryLogic
    {
        public Dictionary<DateTime, TagMonthSummary> GetAllTagDetailedSummary()
        {
            return (new AllTagDetailedSummary()).Get(new DateTime(1900, 1, 1), DateTime.Now);
        }

        public Dictionary<DateTime, TagMonthSummary> GetAllTagDetailedSummary(DateTime startDate, DateTime endDate)
        {
            return (new AllTagDetailedSummary()).Get(startDate, endDate);
        }
    }
}
