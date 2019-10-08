using ExcelWorkerApp.Model;
using ExcelWorkerApp.Utility;
using MoneyStats.DAL.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExcelWorkerApp.Components.ReadExcel
{
    /// <summary>
    /// Read file to memory
    /// </summary>
    class ExcelReader<T> where T : ExcelTransaction, new()
    {
        ConsoleWatch watch;

        public bool IsReadFromTheBeginning { get; set; }

        public ExcelReader()
        {
            this.watch = new ConsoleWatch(this.GetType().Name);
        }

        public ExcelSheet<T> Read(string filePath)
        {
            this.watch.PrintTime($"STARTED.");
            var sheet = new ExcelSheet<T>();
            var groupBuilder = new TransactionGroupBuilder();
            this.ReadExcel(filePath, sheet, groupBuilder);

            // Remaining end-of-dates group
            if (typeof(ExcelTransactionExtended).IsAssignableFrom(typeof(T)))
            {
                foreach (var keyValue in groupBuilder.GroupDict)
                {
                    sheet.AddNewRow(keyValue.Value as T);
                }
            }

            this.watch.PrintDiff($"Document read. DONE.");
            return sheet;
        }

        public ExcelSheet<T> Read(string folderPath, string filePattern)
        {
            this.watch.PrintTime($"STARTED.");
            var sheet = new ExcelSheet<T>();
            List<string> filePaths = this.InitFilePaths(folderPath, filePattern);
            this.watch.PrintDiff($"Paths read. File count: {filePaths.Count}");

            int documentRead = 0;
            var groupBuilder = new TransactionGroupBuilder();
            foreach (string path in filePaths)
            {
                this.ReadExcel(path, sheet, groupBuilder);
                documentRead++;
                this.watch.PrintDiff($"{documentRead}/{filePaths.Count} document{(documentRead > 1 ? "" : "s")} read.");
            }

            // Remaining end-of-dates group
            if (typeof(ExcelTransactionExtended).IsAssignableFrom(typeof(T)))
            {
                foreach (var keyValue in groupBuilder.GroupDict)
                {
                    sheet.AddNewRow(keyValue.Value as T);
                }
            }

            this.watch.PrintDiff($"All documents read. DONE.\n");
            return sheet;
        }

        List<string> InitFilePaths(string folderPath, string filePattern)
        {
            List<string> filePaths = Directory.GetFiles(folderPath, filePattern).ToList();
            filePaths.Sort();
            return filePaths;
        }

        void ReadExcel(
            string path, 
            ExcelSheet<T> excelSheet,
            TransactionGroupBuilder groupBuilder)
        {
            ISheet sheet;

            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.Position = 0;
                if (Path.GetExtension(path) == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                }

                if (excelSheet.IsHeaderEmpty())
                {
                    IRow headerRow = sheet.GetRow(0);
                    this.AddHeaderColumns(excelSheet, headerRow);
                }

                int i = this.GetStartingIndexValue(sheet);

                while ((IsReadFromTheBeginning && i <= sheet.LastRowNum) // from the beginning
                    || (!IsReadFromTheBeginning && i >= (sheet.FirstRowNum + 1))) // from end
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                    {
                        i = this.GetNextIteration(i);
                        continue;
                    }
                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                    {
                        i = this.GetNextIteration(i);
                        continue;
                    }
                    if (row.GetCell(10) != null && row.GetCell(10).ToString() == "1") // IsOmitted column
                    {
                        i = this.GetNextIteration(i);
                        continue;
                    }

                    T tr = new T();

                    if (row.GetCell(0) != null) tr.AccountingDate = row.GetCell(0).DateCellValue;
                    if (row.GetCell(1) != null) tr.TransactionId =  row.GetCell(1).ToString();
                    if (row.GetCell(2) != null) tr.Type =           row.GetCell(2).ToString();
                    if (row.GetCell(3) != null) tr.Account =        row.GetCell(3).ToString();
                    if (row.GetCell(4) != null) tr.AccountName =    row.GetCell(4).ToString();
                    if (row.GetCell(5) != null) tr.PartnerAccount = row.GetCell(5).ToString();
                    if (row.GetCell(6) != null) tr.PartnerName =    row.GetCell(6).ToString();
                    if (row.GetCell(7) != null) tr.Sum =            double.Parse(row.GetCell(7).ToString());
                    if (row.GetCell(8) != null) tr.Currency =       row.GetCell(8).ToString();
                    if (row.GetCell(9) != null) tr.Message =        row.GetCell(9).ToString();

                    if (tr is ExcelTransactionExtended)
                    {
                        groupBuilder.AddPastDatedTransactions(excelSheet as ExcelSheet<ExcelTransactionExtended>, tr.AccountingDate);

                        ExcelTransactionExtended cast = tr as ExcelTransactionExtended;

                        if (row.GetCell(12) != null) cast.TagNames =    this.GetIntList(row.GetCell(12).ToString());
                        if (row.GetCell(13) != null) cast.TagGroupId =  row.GetCell(13).ToString();
                        if (row.GetCell(11) != null)
                        {
                            string groupId = row.GetCell(11).ToString();
                            groupBuilder.StoreCurrentGroupedTransaction(cast, groupId);
                        }
                        else
                        {
                            excelSheet.AddNewRow(cast as T);
                        }
                    }
                    else
                    {
                        excelSheet.AddNewRow(tr);
                    }

                    i = this.GetNextIteration(i);
                }
            }
        }

        #region ReadExcel parts

        int GetStartingIndexValue(ISheet sheet)
        {
            return IsReadFromTheBeginning ? sheet.FirstRowNum + 1 : sheet.LastRowNum;
        }

        int GetNextIteration(int i)
        {
            if (IsReadFromTheBeginning)
            {
                return i + 1;
            }
            return i - 1;
        }

        List<string> GetIntList(string values)
        {
            if (String.IsNullOrWhiteSpace(values))
                return null;

            var list = new List<string>();
            var original = values.Split(',');

            for (int i = 0; i < original.Length; i++)
            {
                list.Add(original[i].Trim());
            }

            return list;
        }

        void AddHeaderColumns(ExcelSheet<T> excelSheet, IRow headerRow)
        {
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                    continue;

                excelSheet.Header.Add(cell.ToString());
            }
        }

        #endregion

        public void TruncateData(ExcelSheet<T> excelSheet)
        {
            this.watch.PrintTime($"STARTED {this.GetType().Name}");
            int originalCount = excelSheet.Transactions.Count;
            int truncatedRowCount = excelSheet.Truncate();
            this.watch.PrintDiff($"FINISHED. {originalCount} - {truncatedRowCount} = {excelSheet.Transactions.Count} row(s) remaining.\n");
        }
    }
}
