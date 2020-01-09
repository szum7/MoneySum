using MoneyStats.DAL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MoneyStats.BL.Model;
using Microsoft.EntityFrameworkCore;
using MoneyStats.BL.TagRepo;

namespace MoneyStats.BL
{
    public class TransactionRepository
    {
        public List<Transaction> Get()
        {
            using(var context = new MoneyStatsContext())
            {
                return (from d in context.Transaction
                        select new Transaction()
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

        public List<Transaction> GetWithEntities()
        {
            using (var context = new MoneyStatsContext())
            {
                var transactions = (
                    from d in context.Transaction
                    orderby d.AccountingDate ascending
                    select new Transaction()
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
                        Currency = d.Currency,
                        Message = d.Message,
                        TransactionTagConn = d.TransactionTagConn
                    })
                    .ToList();

                // Associate tags
                var tags = (new TagRepository()).GetIdKeyedDictionary();
                foreach (var transaction in transactions)
                {
                    foreach (var tranTagConn in transaction.TransactionTagConn)
                    {
                        if (tags.ContainsKey(tranTagConn.TagId))
                        {
                            transaction.Tags.Add(tags[tranTagConn.TagId]);
                        }
                    }
                }

                return transactions;
            }
        }

        string GetTransactionLiteral(Transaction tr)
        {
            return $"{(tr.Sum > 0 ? " " : "")}{tr.Sum}, {tr.AccountingDate.ToShortDateString()}, {tr.PartnerName}";
        }

        public TransactionStat GetTransactionStats()
        {
            // TODO test this
            var transactions = this.GetWithEntities();
            var transactionStat = new TransactionStat();

            transactionStat.Transactions = transactions;

            DateTime? currentDate = null;
            foreach (var transaction in transactions)
            {
                if (!currentDate.HasValue || currentDate.Value.Month != transaction.AccountingDate.Month) // new date
                {
                    currentDate = transaction.AccountingDate;

                    transactionStat.MonthStats.Add(new MonthStat()
                    {
                        Date = new DateTime(currentDate.Value.Year, currentDate.Value.Month, 1),
                        Expense = (transaction.Sum < 0 ? transaction.Sum.Value : 0),
                        Income = (transaction.Sum > 0 ? transaction.Sum.Value : 0),
                        Transactions = new List<Transaction>() { transaction },
                        TransactionLiterals = new List<string>() { this.GetTransactionLiteral(transaction) },
                    });
                }
                else // not new date
                {
                    var lastStat = transactionStat.MonthStats.Last();
                    if (transaction.Sum < 0)
                    {
                        lastStat.Expense += transaction.Sum;
                    }
                    else if (transaction.Sum > 0)
                    {
                        lastStat.Income += transaction.Sum;
                    }
                    lastStat.Transactions.Add(transaction);
                    lastStat.TransactionLiterals.Add(this.GetTransactionLiteral(transaction));
                }
            }

            return transactionStat;
        }

        public void Save(List<Transaction> transactions)
        {
            using (var context = new MoneyStatsContext())
            {
                context.Transaction.AddRange(transactions);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Saves the transactions and all it's associated objects (tags, currency)
        /// </summary>
        /// <param name="transactionsToBeSaved"></param>
        public void SmartSave(List<Transaction> transactionsToBeSaved)
        {
            using (var context = new MoneyStatsContext())
            {
                // Get tags dictionary
                var tagRepo = new TagRepository();
                var tagDict = tagRepo.GetTitleKeyedDictionary();

                // Get not-yet-in-db tags
                var tagsToBeSaved = new List<Tag>();
                foreach (var item in transactionsToBeSaved)
                {
                    if (item.Tags == null)
                        continue;

                    foreach (var tag in item.Tags)
                    {
                        if (!tagDict.ContainsKey(tag.Title) && 
                            !tagsToBeSaved.Any(t => t.Title == tag.Title))
                        {
                            tagsToBeSaved.Add(new Tag()
                            {
                                Title = tag.Title,
                                CreateBy = -1, // TODO get user id
                                CreateDate = DateTime.Now,
                                State = "T"
                            });
                        }
                    }
                }

                // Save newly created tags
                tagRepo.Save(tagsToBeSaved); // Got ids

                // Update tags dictionary
                tagDict = tagRepo.GetTitleKeyedDictionary();

                // Get currency dictionary
                var currencyRepo = new CurrencyRepository();
                var currencyDict = currencyRepo.GetTitleKeyedDictionary();

                // Get not-yet-in-db currencies
                var currenciesToBeSaved = new List<Currency>();
                foreach (var item in transactionsToBeSaved)
                {
                    if (!currencyDict.ContainsKey(item.Currency.Name) && 
                        !currenciesToBeSaved.Any(c => c.Name == item.Currency.Name))
                    {
                        currenciesToBeSaved.Add(item.Currency);
                    }
                }

                // Save newly created currencies
                currencyRepo.Save(currenciesToBeSaved); // Got ids

                // Update transactions' currencyIds
                currencyDict = currencyRepo.GetTitleKeyedDictionary();
                foreach (var transaction in transactionsToBeSaved)
                {
                    transaction.CurrencyId = currencyDict[transaction.Currency.Name];
                    transaction.Currency = null; // Unset since we do not want to save a new Currency (again)
                }

                // Save transactions
                this.Save(transactionsToBeSaved); // Got ids

                // Get not-yet-in-db transactionTagConnections
                var ttcToBeSaved = new List<TransactionTagConn>();
                foreach (var transaction in transactionsToBeSaved)
                {
                    if (transaction.Tags == null)
                        continue;

                    foreach (var tag in transaction.Tags)
                    {
                        ttcToBeSaved.Add(new TransactionTagConn()
                        {
                            TransactionId = transaction.Id,
                            TagId = tagDict[tag.Title] // Should never fail throw keynotfound
                        });
                    }
                }

                // Save transaction-tag connections
                var tranTagConnRepo = new TransactionTagConnRepository();
                tranTagConnRepo.Save(ttcToBeSaved);
            }
        }

        public void DeleteAll()
        {
            using (var context = new MoneyStatsContext())
            {
                context.Database.ExecuteSqlCommand("delete from dbo.[Transaction];DBCC CHECKIDENT ([Transaction], RESEED, 0);");
                context.SaveChanges();
            }
        }
    }
}
