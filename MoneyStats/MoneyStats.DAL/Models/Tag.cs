using System;
using System.Collections.Generic;

namespace MoneyStats.DAL.Models
{
    public partial class Tag
    {
        public Tag()
        {
            TransactionTagConn = new HashSet<TransactionTagConn>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string State { get; set; }

        public virtual ICollection<TransactionTagConn> TransactionTagConn { get; set; }
    }
}
