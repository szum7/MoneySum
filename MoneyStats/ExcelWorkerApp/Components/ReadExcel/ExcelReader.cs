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
    public class ExcelReader<T> where T : ExcelTransaction, new()
    {
        ConsoleWatch watch;
        const string NAME = "ExcelReader";

        public bool IsReadFromTheBeginning { get; set; }

        public ExcelReader()
        {
            this.watch = new ConsoleWatch(NAME);
        }

        public ExcelSheet<T> ReadFile(string filePath)
        {
            this.watch.StartAll();
            this.watch.PrintTime($"Started reading.");
            var sheet = new ExcelSheet<T>();
            this.ReadExcel(filePath, sheet);
            this.watch.PrintTime($"Finished reading the document.\n");
            this.watch.StopAll();
            return sheet;
        }

        public ExcelSheet<T> ReadFiles(string folderPath, string filePattern)
        {
            this.watch.StartAll();
            this.watch.PrintTime($"Started reading.");
            var sheet = new ExcelSheet<T>();
            List<string> filePaths = this.InitFilePaths(folderPath, filePattern);
            this.watch.PrintDiff($"Paths read. File count: {filePaths.Count}");

            int documentRead = 0;
            foreach (string path in filePaths)
            {
                this.ReadExcel(path, sheet);
                documentRead++;
                this.watch.PrintDiff($"{documentRead}/{filePaths.Count} document{(documentRead == 1 ? "" : "s")} read.");
            }

            this.watch.PrintTime($"Finished reading.\n");
            this.watch.StopAll();
            return sheet;
        }

        List<string> InitFilePaths(string folderPath, string filePattern)
        {
            List<string> filePaths = Directory.GetFiles(folderPath, filePattern).ToList();
            filePaths.Sort();
            return filePaths;
        }

        void ReadExcel(string path, ExcelSheet<T> excelSheet)
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
                    if (row == null ||
                        row.Cells.All(d => d.CellType == CellType.Blank))
                    {
                        i = this.GetNextIteration(i);
                        continue;
                    }

                    T tr = new T();

                    if (row.GetCell(0) != null) tr.AccountingDate = row.GetCell(0).DateCellValue;
                    if (row.GetCell(1) != null) tr.TransactionId = row.GetCell(1).ToString().Trim();
                    if (row.GetCell(2) != null) tr.Type = row.GetCell(2).ToString().Trim();
                    if (row.GetCell(3) != null) tr.Account = row.GetCell(3).ToString().Trim();
                    if (row.GetCell(4) != null) tr.AccountName = row.GetCell(4).ToString().Trim();
                    if (row.GetCell(5) != null) tr.PartnerAccount = row.GetCell(5).ToString().Trim();
                    if (row.GetCell(6) != null) tr.PartnerName = row.GetCell(6).ToString().Trim();
                    if (row.GetCell(7) != null) tr.Sum = double.Parse(row.GetCell(7).ToString());
                    if (row.GetCell(8) != null) tr.Currency = row.GetCell(8).ToString().Trim();
                    if (row.GetCell(9) != null) tr.Message = row.GetCell(9).ToString().Trim();

                    if (tr is ExcelTransactionExtended)
                    {
                        ExcelTransactionExtended cast = tr as ExcelTransactionExtended;

                        if (row.GetCell(10) != null) cast.IsOmitted = row.GetCell(10).ToString().Trim() == "1";
                        if (row.GetCell(11) != null) cast.GroupId = row.GetCell(11).ToString().Trim();
                        if (row.GetCell(12) != null) cast.TagNames = this.GetIntList(row.GetCell(12).ToString().Trim());
                        if (row.GetCell(13) != null) cast.TagGroupId = row.GetCell(13).ToString().Trim();                        
                    }

                    excelSheet.AddNewRow(tr);
                    i = this.GetNextIteration(i);
                }
            }
        }

        #region ReadExcel utilities

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

                excelSheet.Header.Add(cell.ToString().Trim());
            }
        }

        #endregion

        public void TruncateData(ExcelSheet<T> excelSheet)
        {
            this.watch.StartAll();
            this.watch.PrintTime($"Started truncating.");
            int originalCount = excelSheet.Transactions.Count;
            int truncatedRowCount = excelSheet.Truncate();
            this.watch.PrintDiff($"Finished truncating. {originalCount} - {truncatedRowCount} = {excelSheet.Transactions.Count} row{(excelSheet.Transactions.Count <= 1 ? "" : "s")} remaining.\n");
            this.watch.StopAll();
        }
    }
}
