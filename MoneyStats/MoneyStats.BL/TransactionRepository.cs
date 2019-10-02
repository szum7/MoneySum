﻿using MoneyStats.DAL;
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

        public List<TransactionModel> GetWithEntities()
        {
            using (var context = new DBContext())
            {
                var tagsRepo = new TagRepository();
                var tags = tagsRepo.Get();

                var transactions = (
                    from d in context.Transaction
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
                        Currency = d.Currency,
                        Message = d.Message,
                        TransactionTagConn = d.TransactionTagConn
                    })
                    .ToList();

                // Associate tags
                foreach (var transaction in transactions)
                {
                    foreach (var tranTagConn in transaction.TransactionTagConn)
                    {
                        var tag = tags.SingleOrDefault(x => x.Id == tranTagConn.TagId);
                        if (tag != null)
                        {
                            transaction.Tags.Add(tag);
                        }
                    }
                }


                return transactions;
            }
        }
        
        public void Save(IEnumerable<TransactionModel> transactions)
        {
            using (var context = new DBContext())
            {
                context.Transaction.AddRange(transactions);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Saves the transactions and all it's associated objects (tags, currency)
        /// </summary>
        /// <param name="transactionsToBeSaved"></param>
        public void SmartSave(IEnumerable<TransactionModel> transactionsToBeSaved)
        {
            using (var context = new DBContext())
            {
                // Get tags dictionary
                var tagRepo = new TagRepository();
                var tagDict = tagRepo.GetTitleKeyedDictionary();

                // Get not-yet-in-db tags
                var tagsToBeSaved = new List<TagModel>();
                foreach (var item in transactionsToBeSaved)
                {
                    foreach (var tag in item.Tags)
                    {
                        if (!tagDict.ContainsKey(tag.Title))
                        {
                            tagsToBeSaved.Add(new TagModel()
                            {
                                Title = tag.Title,
                                CreateBy = -1, // TODO get user id
                                CreateDate = DateTime.Now
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
                var currenciesToBeSaved = new List<CurrencyModel>();
                foreach (var item in transactionsToBeSaved)
                {
                    if (!currencyDict.ContainsKey(item.Currency.Name))
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
                }

                // Save transactions
                this.Save(transactionsToBeSaved); // Got ids

                // Get not-yet-in-db transactionTagConnections
                var ttcToBeSaved = new List<TransactionTagConnModel>();
                foreach (var transaction in transactionsToBeSaved)
                {
                    foreach (var tag in transaction.Tags)
                    {
                        ttcToBeSaved.Add(new TransactionTagConnModel()
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
    }
}
