using ExcelWorkerApp.Components.ReadExcel;
using ExcelWorkerApp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MoneyStats.Tests.ExcelReaderTesters
{
    [TestClass]
    public class ExcelReaderTester
    {
        [TestMethod]
        public void TestMergedFileRead()
        {
            // Arrange
            var reader = new ExcelReader<ExcelTransactionExtended>();
            var result = new ExcelSheet<ExcelTransactionExtended>();
            #region Set result
            result.Header = new List<string>() { "Könyvelés dátuma", "Tranzakció azonosító", "Típus", "Könyvelési számla", "Könyvelési számla elnevezése", "Partner számla", "Partner elnevezése", "Összeg", "Összeg devizaneme", "Közlemény", "Is omitted?", "Group id", "Tag names", "Tag group id" };
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2010, 10, 10), TransactionId = "tr-az-1", Type = "tí-1", Account = "kö-szá-1", AccountName = "kö-szá-el-1", PartnerAccount = "pa-szá-1", PartnerName = "pa-el-1", Sum = 10, Currency = "HUF", Message = "kö-1", GroupId = null, TagNames = new List<string>(), TagGroupId = null });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2010, 10, 13), TransactionId = "tr-az-3", Type = "tí-3", Account = "kö-szá-3", AccountName = "kö-szá-el-3", PartnerAccount = "pa-szá-3", PartnerName = "pa-el-3", Sum = 30, Currency = "HUF", Message = "kö-3", GroupId = null, TagNames = new List<string>(), TagGroupId = null });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2010, 10, 31), TransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 90, Currency = "HUF", Message = null, GroupId = "1", TagNames = new List<string>() { "a", "b", "c" }, TagGroupId = "1" });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2011, 1, 31), TransactionId = "tr-az-6", Type = "tí-6", Account = "kö-szá-6", AccountName = "kö-szá-el-6", PartnerAccount = "pa-szá-6", PartnerName = "pa-el-6", Sum = 60, Currency = "HUF", Message = "kö-6", GroupId = null, TagNames = new List<string>(), TagGroupId = null });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2011, 2, 4), TransactionId = "tr-az-7", Type = "tí-7", Account = "kö-szá-7", AccountName = "kö-szá-el-7", PartnerAccount = "pa-szá-7", PartnerName = "pa-el-7", Sum = 70, Currency = "HUF", Message = "kö-7", GroupId = null, TagNames = new List<string>(), TagGroupId = null });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2011, 2, 18), TransactionId = "tr-az-11", Type = "tí-11", Account = "kö-szá-11", AccountName = "kö-szá-el-11", PartnerAccount = "pa-szá-11", PartnerName = "pa-el-11", Sum = 110, Currency = "HUF", Message = "kö-11", GroupId = null, TagNames = new List<string>() { "a", "b" }, TagGroupId = null });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2011, 2, 28), TransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 170, Currency = "HUF", Message = null, GroupId = "1", TagNames = new List<string>() { "a", "b", "c" }, TagGroupId = "1" });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2011, 2, 28), TransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 100, Currency = "HUF", Message = null, GroupId = "2", TagNames = new List<string>(), TagGroupId = null });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2011, 3, 31), TransactionId = "tr-az-13", Type = "tí-13", Account = "kö-szá-13", AccountName = "kö-szá-el-13", PartnerAccount = "pa-szá-13", PartnerName = "pa-el-13", Sum = 130, Currency = "HUF", Message = "kö-13", GroupId = null, TagNames = new List<string>() { "g" }, TagGroupId = "2" });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2011, 3, 31), TransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 120, Currency = "HUF", Message = null, GroupId = "1", TagNames = new List<string>() { "a", "b", "c" }, TagGroupId = "1" });
            result.Transactions.Add(new ExcelTransactionExtended() { AccountingDate = new DateTime(2016, 6, 30), TransactionId = null, Type = null, Account = null, AccountName = null, PartnerAccount = null, PartnerName = null, Sum = 200, Currency = "HUF", Message = null, GroupId = "2", TagNames = new List<string>(), TagGroupId = null });
            #endregion

            // Act
            reader.IsReadFromTheBeginning = true;

            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
            var excelSheet = reader.ReadFile($@"{path}\ExcelReaderTester\Files\ExelReaderTestFile.xlsx");
            excelSheet.RemoveOmittedRows().ApplyTagsToTagGroups().ApplyGroups();
            var str1 = JsonConvert.SerializeObject(excelSheet);
            var str2 = JsonConvert.SerializeObject(result);

            // Assert
            Assert.AreEqual(str1, str2);
        }
    }
}
