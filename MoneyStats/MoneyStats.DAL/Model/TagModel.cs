using System.Collections.Generic;

namespace MoneyStats.DAL.Model
{
    public class TagModel: DBModel
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<TransactionTagConnModel> TransactionTagConn { get; set; }

        public TagModel()
        {
            this.TransactionTagConn = new List<TransactionTagConnModel>();
        }
    }
}
