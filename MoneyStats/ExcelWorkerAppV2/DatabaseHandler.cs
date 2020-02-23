using ExcelWorkerApp.Model;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace ExcelWorkerAppV2
{
    class DatabaseHandler
    {
        public static ExcelSheet ConvertTransactionListToExcelSheet(List<Transaction> dbData)
        {
            var data = new ExcelSheet();
            foreach (var item in dbData)
            {
                data.AddNewRow(new ExcelTransaction()
                {
                    AccountingDate = item.AccountingDate,
                    TransactionId = item.TransactionId,
                    Type = item.Type,
                    Account = item.Account,
                    AccountName = item.AccountName,
                    PartnerAccount = item.PartnerAccount,
                    PartnerName = item.PartnerName,
                    Sum = (double)item.Sum.Value,
                    Currency = item.Currency.Name,
                    Message = item.Message
                });
            }
            return data;
        }
    }
}
