using System;
using System.Collections.Generic;

namespace MoneyStats.DAL.Models
{
    public partial class Currency
    {
        public Currency()
        {
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string State { get; set; }

        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
