using ExcelWorkerApp.Model;
using MoneyStats.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelWorkerApp.Components.MergeTransaction
{
    class TransactionMerger
    {
        public List<TransactionModel> Run(List<TransactionModel> tr1, List<ExcelTransactionExtended> tr2)
        {
            // Order by date
            tr1.OrderBy(x => x.AccountingDate);
            tr2.OrderBy(x => x.AccountingDate);

            var groupDict = this.GetTagGroupDictionary(tr2);

            // Merge
            List<TransactionModel> merged = new List<TransactionModel>();
            int i = 0, j = 0;
            while (i < tr1.Count && j < tr2.Count)
            {
                if (tr1[i].AccountingDate < tr2[j].AccountingDate)
                {
                    merged.Add(tr1[i]);
                    i++;
                }
                else if (tr1[i].AccountingDate > tr2[j].AccountingDate)
                {
                    merged.Add(this.ConvertToModel(tr2[j], groupDict));
                    j++;
                }
                else
                {
                    merged.Add(tr1[i]);
                    if (tr1[i].ContentId != tr2[j].ContentId)
                    {
                        merged.Add(this.ConvertToModel(tr2[j], groupDict));
                    }
                    i++;
                    j++;

                }
            }
            while (i < tr1.Count)
            {
                merged.Add(tr1[i]);
                i++;
            }
            while (j < tr2.Count)
            {
                merged.Add(this.ConvertToModel(tr2[j], groupDict));
                j++;
            }

            return merged;
        }

        public IEnumerable<TransactionModel> GetNewRows(List<TransactionModel> list)
        {
            return list.Where(x => x.Id == -1);
        }

        Dictionary<string, List<string>> GetTagGroupDictionary(List<ExcelTransactionExtended> list)
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

        List<TagModel> GetTagModelListByTagGroupId(ExcelTransactionExtended transaction, Dictionary<string, List<string>> tagGroupDict)
        {
            var tags = new List<TagModel>();
            if (tagGroupDict == null || tagGroupDict.Count == 0)
            {
                Console.WriteLine("New row's TagGroupId is set, but the dictionary contains no elements!");
            }
            else if (!tagGroupDict.ContainsKey(transaction.TagGroupId))
            {
                Console.WriteLine("The new row's TagGroupId was not found in the dictionary!");
            }
            else // Has TagGroupId, create a TagModel list
            {
                var groupNames = tagGroupDict[transaction.TagGroupId];
                foreach (var item in groupNames)
                {
                    tags.Add(new TagModel()
                    {
                        Id = -1,
                        Title = item
                    });
                }
            }
            return tags.Count > 0 ? tags : null;
        }

        List<TagModel> GetTagModelList(ExcelTransactionExtended transaction)
        {
            var tags = new List<TagModel>();
            foreach (var item in transaction.TagNames)
            {
                tags.Add(new TagModel()
                {
                    Id = -1,
                    Title = item
                });
            }
            return tags.Count > 0 ? tags : null;
        }

        TransactionModel ConvertToModel(ExcelTransactionExtended tr, Dictionary<string, List<string>> tagGroupDict)
        {
            List<TagModel> tags = null;
            if (String.IsNullOrWhiteSpace(tr.GroupId))
            {
                tags = this.GetTagModelList(tr);
            }
            else
            {
                tags = this.GetTagModelListByTagGroupId(tr, tagGroupDict);
            }

            return new TransactionModel()
            {
                Id = -1,
                Account = tr.Account,
                AccountName = tr.AccountName,
                AccountingDate = tr.AccountingDate,
                Type = tr.Type,
                TransactionId = tr.TransactionId,
                PartnerAccount = tr.PartnerAccount,
                PartnerName = tr.PartnerName,
                Sum = (decimal?)tr.Sum,
                Message = tr.Message,
                Tags = tags,
                Currency = new CurrencyModel()
                {
                    Id = -1,
                    Name = tr.Currency
                }
            };
        }
    }
}
