using MoneyStats.DAL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MoneyStats.DAL;

namespace MoneyStats.BL
{
    public class CurrencyRepository
    {
        public List<CurrencyModel> Get()
        {
            using (var context = new DBContext())
            {
                return (from d in context.Currency
                        select new CurrencyModel()
                        {
                            Id = d.Id,
                            Name = d.Name
                        }).ToList();
            }
        }
    }
}
