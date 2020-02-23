using ExcelWorkerApp.Model;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelWorkerAppV2
{
    public class TransactionConverter
    {
        int newTagId = -1;

        public List<Transaction> ConvertToTransactionModel(List<ExcelTransaction> excelTransactions)
        {
            var list = new List<Transaction>();
            var groupDict = GetTagGroupDictionary(excelTransactions);

            foreach (var transaction in excelTransactions)
            {
                var model = ConvertToModel(transaction);
                model.Tags = GetTagModelList(transaction.TagGroupId, transaction.TagNames, groupDict);
                list.Add(model);
            }

            return list;
        }

        Dictionary<string, List<string>> GetTagGroupDictionary(List<ExcelTransaction> list)
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

        public Transaction ConvertToModel(ExcelTransaction tr)
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

        List<Tag> GetTagModelList(string tagGroupId, List<string> tagNames, Dictionary<string, List<string>> tagGroupDict)
        {
            List<Tag> tags;
            if (String.IsNullOrWhiteSpace(tagGroupId))
            {
                tags = GetTagModelList(tagNames);
            }
            else
            {
                tags = GetTagModelListByTagGroupId(tagGroupId, tagGroupDict);
            }
            return tags;
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
    }
}
