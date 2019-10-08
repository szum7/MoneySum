using ExcelWorkerApp.Components.MergeTransaction;
using ExcelWorkerApp.Components.ReadExcel;
using ExcelWorkerApp.Components.WriteExcel;
using ExcelWorkerApp.Model;
using MoneyStats.BL;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this.bankExportedTransactions = excelReader.Read(folderPath, filePattern);
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

        public void ReadExtendedTransactionsMergedFile(string fullFilePath)
        {
            mergedFileReader.IsReadFromTheBeginning = true;
            // @"C:\Users\Aron_Szocs\Documents\Bank\Merged\Merged.xls"
            this.extendedMergedTransactions = this.mergedFileReader.Read(fullFilePath);
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
    }
}
