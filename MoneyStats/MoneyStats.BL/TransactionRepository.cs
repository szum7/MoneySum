using MoneyStats.DAL;
using MoneyStats.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyStats.BL
{
    public class TransactionRepository
    {
        public List<TransactionModel> Get()
        {
            using(var context = new DBContext())
            {
                return (from d in context.Transaction
                        select new TransactionModel()
                        {
                            Id = d.Id,
                            AccountingDate = d.AccountingDate,
                            TransactionId = d.TransactionId,
                            Type = d.Type,
                            Account = d.Account,
                            AccountName = d.AccountName,
                            PartnerAccount = d.PartnerAccount,
                            PartnerName = d.PartnerName,
                            Sum = d.Sum,
                            CurrencyId = d.CurrencyId,
                            Message = d.Message
                        }).ToList();
            }
        }
    }
}
