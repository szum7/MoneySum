using ExcelWorkerApp.Components.MergeTransaction;
using ExcelWorkerApp.Components.ReadExcel;
using ExcelWorkerApp.Components.WriteExcel;
using ExcelWorkerApp.Model;
using MoneyStats.BL;
using MoneyStats.DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelWorkerApp
{
    public class ProgramRunner
    {
        ExcelReader<ExcelTransaction> excelReader;
        ExcelWriter excelWriter;
        ExcelReader<ExcelTransactionExtended> mergedFileReader;
        TransactionMerger transactionMerger;
        TransactionRepository transactionRepo;

        ExcelSheet<ExcelTransaction> bankExportedTransactions;
        ExcelSheet<ExcelTransactionExtended> extendedMergedTransactions;
        List<Transaction> databaseTransactions;
        List<Transaction> mergedDatabaseTransactions;
        List<Transaction> unsavedTransactionList;

        public ProgramRunner()
        {
            this.excelReader = new ExcelReader<ExcelTransaction>();
            this.excelWriter = new ExcelWriter();
            this.mergedFileReader = new ExcelReader<ExcelTransactionExtended>();
            this.transactionMerger = new TransactionMerger();
            this.transactionRepo = new TransactionRepository();
        }

        public void ReadManyBankExportedFiles(string folderPath, string filePattern)
        {
            // @"C:\Users\Aron_Szocs\Documents\Bank", "*.xls"
            this.bankExportedTransactions = excelReader.ReadFiles(folderPath, filePattern);
        }

        public void TruncateBankExportedFiles()
        {
            this.bankExportedTransactions.Transactions = this.bankExportedTransactions.Transactions.OrderBy(t => t.AccountingDate).ToList();
            this.excelReader.TruncateData(this.bankExportedTransactions);
        }

        public void CreateMergedExcelFile(string fullFilePath)
        {
            if (this.bankExportedTransactions == null || 
                this.bankExportedTransactions.Transactions.Count == 0)
            {
                Console.WriteLine("[ALERT] Transactions are empty!");
                return;
            }
            //@"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls"
            this.excelWriter.Run(this.bankExportedTransactions, fullFilePath);
        }

        public void CreateExtendedMergedExcelFile(string fullFilePath)
        {
            if (this.extendedMergedTransactions == null ||
                this.extendedMergedTransactions.Transactions.Count == 0)
            {
                Console.WriteLine("[ALERT] Transactions are empty!");
                return;
            }
            this.excelWriter.Run(this.extendedMergedTransactions, fullFilePath);
        }

        public void ReadLastExtendedTransactionsMergedFile(string mergedFilesPath)
        {
            List<string> filePaths = Directory.GetFiles(mergedFilesPath, "*.xls").ToList();
            filePaths.Sort();
            this.mergedFileReader.IsReadFromTheBeginning = true;
            this.extendedMergedTransactions = this.mergedFileReader.ReadFile(filePaths.Last());
        }

        public void MergeLastExtendedTransactionsWithNewlyReadOnes()
        {
            //var castList = this.bankExportedTransactions.Transactions.Cast<ExcelTransactionExtended>().ToList();
            var serializedParent = JsonConvert.SerializeObject(this.bankExportedTransactions.Transactions);
            var castList = JsonConvert.DeserializeObject<List<ExcelTransactionExtended>>(serializedParent);
            this.extendedMergedTransactions.MergeWith(castList);
        }

        public void ReadAndHandleExtendedTransactionsMergedFile(string fullFilePath)
        {
            this.mergedFileReader.IsReadFromTheBeginning = true;
            this.extendedMergedTransactions = this.mergedFileReader.ReadFile(fullFilePath);
            this.extendedMergedTransactions
                .RemoveOmittedRows()
                .ApplyTagsToTagGroups()
                .ApplyGroups();
        }

        public void LoadAlreadyExistingTransactionsFromDatabase()
        {
            this.databaseTransactions = this.transactionRepo.GetWithEntities();
        }

        public void MergeDatabaseAndExtendedMergedTransactions()
        {
            this.mergedDatabaseTransactions = transactionMerger.Run(this.databaseTransactions, extendedMergedTransactions.Transactions);
            //excelWriter.Run(mergedList, @"C:\Users\Aron_Szocs\Documents\Bank\Merged\MergedList.xls");
            this.unsavedTransactionList = transactionMerger.GetNewRows(this.mergedDatabaseTransactions);
        }

        public void SaveNewBankExportedTransactions()
        {
            this.transactionRepo.SmartSave(this.unsavedTransactionList);
        }

        public void SaveBankExportedTransactions()
        {
            // Convert excel transactions to db transaction model.
            // TODO - make the converting separate from the merger
            List<Transaction> convertedExcelTransactions = transactionMerger.Run(new List<Transaction>(), extendedMergedTransactions.Transactions);

            this.transactionRepo.SmartSave(convertedExcelTransactions);
        }

        public void ClearTransactionRelatedDataFromDatabase()
        {
            var transTagConnRepo = new TransactionTagConnRepository();
            var transRepo = new TransactionRepository();
            var tagRepo = new TagRepository();
            var currencyRepo = new CurrencyRepository();

            transTagConnRepo.DeleteAll();
            transRepo.DeleteAll();
            tagRepo.DeleteAll();
            currencyRepo.DeleteAll();
        }
    }
}
