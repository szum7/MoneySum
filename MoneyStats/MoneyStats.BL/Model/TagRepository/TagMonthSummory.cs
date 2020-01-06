using System;
using System.Collections.Generic;

namespace MoneyStats.BL.Model.TagRepository
{
    public class MonthSummary
    {
        public DateTime Month { get; set; }
        public List<TagMonthSummaryLine> Tags { get; set; }
        public List<TagInstance> TagInstances { get; set; } // Unused
    }
}
