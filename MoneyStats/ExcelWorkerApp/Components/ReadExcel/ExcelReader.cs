﻿using ExcelWorkerApp.Model;
using ExcelWorkerApp.Utility;
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
    class ExcelReader<T> where T : Transaction, new()
    {
        ExcelSheet<T> sheet;
        ConsoleWatch watch;

        public bool IsReadFromTheBeginning { get; set; }

        public ExcelReader()
        {
            this.sheet = new ExcelSheet<T>();
            this.watch = new ConsoleWatch(this.GetType().Name);
        }

        public ExcelSheet<T> Read(string filePath)
        {
            this.watch.PrintTime($"STARTED.");
            this.ReadExcel(filePath, 1);
            this.watch.PrintDiff($"Document read. DONE.");
            return this.sheet;
        }

        public ExcelSheet<T> Read(string folderPath, string filePattern)
        {
            this.watch.PrintTime($"STARTED.");
            List<string> filePaths = this.InitFilePaths(folderPath, filePattern);
            this.watch.PrintDiff($"Paths read. File count: {filePaths.Count}");

            int documentRead = 0;
            int rowId = 1;
            foreach (string path in filePaths)
            {
                rowId = this.ReadExcel(path, rowId);
                documentRead++;
                this.watch.PrintDiff($"{documentRead}/{filePaths.Count} document{(documentRead > 1 ? "" : "s")} read.");
            }
            this.watch.PrintDiff($"All documents read. DONE.\n");
            return this.sheet;
        }

        List<string> InitFilePaths(string folderPath, string filePattern)
        {
            List<string> filePaths = Directory.GetFiles(folderPath, filePattern).ToList();
            filePaths.Sort();
            return filePaths;
        }

        int ReadExcel(string path, int rowId)
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

                if (this.sheet.IsHeaderEmpty())
                {
                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;
                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                            continue;

                        this.sheet.Header.Add(cell.ToString());
                    }
                }

                int i = sheet.LastRowNum;
                if (IsReadFromTheBeginning)
                {
                    i = sheet.FirstRowNum + 1;
                }

                while ((IsReadFromTheBeginning && i <= sheet.LastRowNum) || (!IsReadFromTheBeginning && i > (sheet.FirstRowNum + 1)))
                //for (int i = sheet.LastRowNum; i > (sheet.FirstRowNum + 1); i--) //Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null || row.Cells.All(d => d.CellType == CellType.Blank))
                    {
                        i = this.GetNextIteration(i);
                        continue;
                    }

                    T tr = new T();

                    tr.Id = rowId;
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

                    if (tr is TransactionExtended)
                    {
                        TransactionExtended cast = tr as TransactionExtended;

                        if (row.GetCell(10) != null) cast.IsOmitted =   row.GetCell(10).ToString() == "1";
                        if (row.GetCell(11) != null) cast.GroupId =     row.GetCell(11).ToString();
                        if (row.GetCell(12) != null) cast.TagNames =    this.GetIntList(row.GetCell(12).ToString());
                        if (row.GetCell(13) != null) cast.TagGroupId =  row.GetCell(13).ToString();
                    }

                    this.sheet.AddNewRow(tr);

                    rowId++;

                    i = this.GetNextIteration(i);
                }
            }
            return rowId;
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

        public void TruncateData()
        {
            this.watch.PrintTime($"STARTED {this.GetType().Name}");
            int truncatedRowCount = this.sheet.Truncate();
            this.watch.PrintDiff($"FINISHED. {truncatedRowCount} truncated rows, {this.sheet.Transactions.Count} remaining.\n");
        }
    }
}