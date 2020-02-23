using ExcelWorkerApp.Model;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelWorkerAppV2
{
    class TransactionMerger
    {
        int newTagId = -1;

        [Obsolete]
        public List<Transaction> Run(List<Transaction> tr1, List<ExcelTransaction> tr2)
        {
            // Order lists on dates
            tr1 = tr1.OrderBy(x => x.AccountingDate).ToList();
            tr2 = tr2.OrderBy(x => x.AccountingDate).ToList();

            // Init group dictionary to create tag model lists faster
            var groupDict = this.InitTagGroupDictionary(tr2);

            // Merge
            var merged = new List<Transaction>();
            int i = 0;
            int j = 0;
            while (i < tr1.Count && j < tr2.Count)
            {
                if (tr1[i].AccountingDate < tr2[j].AccountingDate)
                {
                    merged.Add(tr1[i]);
                    i++;
                }
                else if (tr1[i].AccountingDate > tr2[j].AccountingDate)
                {
                    this.AddExcelTransactionToTransactionModelList(merged, tr2[j], groupDict);
                    j++;
                }
                else
                {
                    // Transactions are not ordered on any other than dates
                    // therefor once dates are the same, we need to check
                    // if a transaction is unique on every iteration.
                    var interval = new List<Transaction>();
                    var date = tr1[i].AccountingDate;
                    while (i < tr1.Count && tr1[i].AccountingDate == date)
                    {
                        if (!interval.Any(x => x.ContentId == tr1[i].ContentId))
                        {
                            interval.Add(tr1[i]);
                        }
                        i++;
                    }
                    while (j < tr2.Count && date == tr2[j].AccountingDate)
                    {
                        if (!interval.Any(x => x.ContentId == tr2[j].ContentId))
                        {
                            this.AddExcelTransactionToTransactionModelList(interval, tr2[j], groupDict);
                        }
                        j++;
                    }
                    merged.AddRange(interval);
                }
            }
            while (i < tr1.Count)
            {
                merged.Add(tr1[i]);
                i++;
            }
            while (j < tr2.Count)
            {
                this.AddExcelTransactionToTransactionModelList(merged, tr2[j], groupDict);
                j++;
            }
            
            return merged;
        }

        [Obsolete]
        public List<Transaction> GetNewRows(List<Transaction> list)
        {
            return list.Where(x => x.Id <= 0).ToList();
        }

        void AddExcelTransactionToTransactionModelList(List<Transaction> list, ExcelTransaction transaction, Dictionary<string, List<string>> tagGroupDict)
        {
            var model = this.ConvertToModel(transaction);
            model.Tags = this.GetTagModelList(transaction.TagGroupId, transaction.TagNames, tagGroupDict);
            list.Add(model);
        }

        Dictionary<string, List<string>> InitTagGroupDictionary(List<ExcelTransaction> list)
        {
            var dict = new Dictionary<string, List<string>>();

            foreach (var item in list)
            {
                if (!String.IsNullOrWhiteSpace(item.TagGroupId) && 
                    !dict.ContainsKey(item.TagGroupId) && 
                    item.TagNames.Count > 0)
                {
                    dict.Add(item.TagGroupId, item.TagNames);
                }
            }

            return dict;
        }

        List<Tag> GetTagModelListByTagGroupId(string tagGroupId, Dictionary<string, List<string>> tagGroupDict)
        {
            var tags = new List<Tag>();
            if (tagGroupDict == null || tagGroupDict.Count == 0)
            {
                Console.WriteLine("New row's TagGroupId is set, but the dictionary contains no elements!");
            }
            else if (!tagGroupDict.ContainsKey(tagGroupId))
            {
                Console.WriteLine("The new row's TagGroupId was not found in the dictionary!");
            }
            else // Has TagGroupId, create a TagModel list
            {
                var groupNames = tagGroupDict[tagGroupId];
                foreach (var item in groupNames)
                {
                    tags.Add(new Tag()
                    {
                        Id = this.newTagId--,
                        Title = item
                    });
                }
            }
            return tags.Count > 0 ? tags : null;
        }

        List<Tag> GetTagModelList(List<string> tagNames)
        {
            var tags = new List<Tag>();
            foreach (var tagName in tagNames)
            {
                tags.Add(new Tag()
                {
                    Id = this.newTagId--,
                    Title = tagName
                });
            }
            return tags.Count > 0 ? tags : null;
        }

        List<Tag> GetTagModelList(string tagGroupId, List<string> tagNames, Dictionary<string, List<string>> tagGroupDict)
        {
            List<Tag> tags;
            if (String.IsNullOrWhiteSpace(tagGroupId))
            {
                tags = this.GetTagModelList(tagNames);
            }
            else
            {
                tags = this.GetTagModelListByTagGroupId(tagGroupId, tagGroupDict);
            }
            return tags;
        }

        Transaction ConvertToModel(ExcelTransaction tr)
        {
            return new Transaction()
            {
                //Id = default(int),
                Account = tr.Account,
                AccountName = tr.AccountName,
                AccountingDate = tr.AccountingDate,
                Type = tr.Type,
                TransactionId = tr.TransactionId,
                PartnerAccount = tr.PartnerAccount,
                PartnerName = tr.PartnerName,
                Sum = (decimal?)tr.Sum,
                Message = tr.Message,
                Tags = null,
                Currency = new Currency()
                {
                    //Id = this.newTransactionId--,
                    Name = tr.Currency,
                    CreateDate = DateTime.Now,
                    State = "T"
                },
                CreateDate = DateTime.Now,
                State = "T"
            };
        }
    }
}
