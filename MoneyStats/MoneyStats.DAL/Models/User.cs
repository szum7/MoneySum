using System;
using System.Collections.Generic;

namespace MoneyStats.DAL.Models
{
    public partial class User
    {
        public User()
        {
            Setting = new HashSet<Setting>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string State { get; set; }

        public virtual ICollection<Setting> Setting { get; set; }
    }
}
