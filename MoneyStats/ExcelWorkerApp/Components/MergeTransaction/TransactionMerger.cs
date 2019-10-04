using ExcelWorkerApp.Model;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelWorkerApp.Components.MergeTransaction
{
    class TransactionMerger
    {
        public List<Transaction> Run(List<Transaction> tr1, List<ExcelTransactionExtended> tr2)
        {
            // Order by date
            tr1 = tr1.OrderBy(x => x.AccountingDate).ToList();
            tr2 = tr2.OrderBy(x => x.AccountingDate).ToList();

            var groupDict = this.GetTagGroupDictionary(tr2);

            // Merge
            List<Transaction> merged = new List<Transaction>();
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
                    while (i < tr1.Count && j < tr2.Count && tr1[i].AccountingDate == tr2[j].AccountingDate)
                    {
                        if (!merged.Any(x => x.ContentId == tr1[i].ContentId))
                        {
                            merged.Add(tr1[i]);
                        }
                        if (tr1[i].ContentId != tr2[j].ContentId)
                        {
                            if (!merged.Any(x => x.ContentId == tr2[j].ContentId))
                            {
                                merged.Add(this.ConvertToModel(tr2[j], groupDict));
                            }
                            j++;
                        }
                        i++;
                    }


                    //var bool1 = !merged.Any(x => x.ContentId == tr1[i].ContentId);
                    //var bool2 = !merged.Any(x => x.ContentId == tr2[j].ContentId);
                    //if (bool1)
                    //{
                    //    merged.Add(tr1[i]);
                    //}
                    //if (bool2)
                    //{
                    //    merged.Add(this.ConvertToModel(tr2[j], groupDict));
                    //}
                    //if (!bool1 && !bool2)
                    //{

                    //}


                    //var find = tr2.Find(x => x.ContentId == tr1[i].ContentId);


                    //var a = i;
                    //while (a < tr1.Count
                    //    && tr1[a].AccountingDate == tr1[i].AccountingDate
                    //    && tr1[a].ContentId != tr2[j].ContentId)
                    //{
                    //    a++;
                    //}
                    //if (a >= tr1.Count ||  
                    //    tr1[a].AccountingDate != tr1[i].AccountingDate) // nem talált egyelőt a tartomány jobb részén
                    //{
                    //    a = i;
                    //    while (a >= 0
                    //    && tr1[a].AccountingDate == tr1[i].AccountingDate
                    //    && tr1[a].ContentId != tr2[j].ContentId)
                    //    {
                    //        a--;
                    //    }

                    //    if (a < 0 ||
                    //        tr1[a].AccountingDate != tr1[i].AccountingDate) // nem talált egyelőt a tartományban bal részén
                    //    {
                    //        merged.Add(this.ConvertToModel(tr2[j], groupDict));
                    //    }
                    //}



                    //if (tr1[i].ContentId != tr2[j].ContentId)
                    //{
                    //    merged.Add(this.ConvertToModel(tr2[j], groupDict));
                    //}
                    //else
                    //{

                    //}
                    //i++;
                    //j++;

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

        public List<Transaction> GetNewRows(List<Transaction> list)
        {
            return list.Where(x => x.Id < 1).ToList();
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

        int newTagId = -1;

        List<Tag> GetTagModelListByTagGroupId(ExcelTransactionExtended transaction, Dictionary<string, List<string>> tagGroupDict)
        {
            var tags = new List<Tag>();
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
                    tags.Add(new Tag()
                    {
                        Id = this.newTagId--,
                        Title = item
                    });
                }
            }
            return tags.Count > 0 ? tags : null;
        }

        List<Tag> GetTagModelList(ExcelTransactionExtended transaction)
        {
            var tags = new List<Tag>();
            foreach (var item in transaction.TagNames)
            {
                tags.Add(new Tag()
                {
                    Id = this.newTagId--,
                    Title = item
                });
            }
            return tags.Count > 0 ? tags : null;
        }

        int newTransactionId = -1;

        Transaction ConvertToModel(ExcelTransactionExtended tr, Dictionary<string, List<string>> tagGroupDict)
        {
            List<Tag> tags = null;
            if (String.IsNullOrWhiteSpace(tr.GroupId))
            {
                tags = this.GetTagModelList(tr);
            }
            else
            {
                tags = this.GetTagModelListByTagGroupId(tr, tagGroupDict);
            }

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
                Tags = tags,
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
