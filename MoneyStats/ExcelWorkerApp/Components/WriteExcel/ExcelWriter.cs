using ExcelWorkerApp.Model;
using ExcelWorkerApp.Utility;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System;

namespace ExcelWorkerApp.Components.WriteExcel
{
    /// <summary>
    /// Create a file from memory
    /// TODO: make it generic if possible/logical
    /// </summary>
    class ExcelWriter
    {
        ConsoleWatch watch;

        public ExcelWriter()
        {
            this.watch = new ConsoleWatch(this.GetType().Name);
        }

        public void Run(ExcelSheet<Transaction> excelData, string filePath, bool isCreateExtendedHeader = true)
        {
            this.watch.PrintTime($"STARTED.");

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                if (Path.GetExtension(filePath) == ".xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
                else
                {
                    workbook = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());
                }
                this.watch.PrintDiff($"File created.");

                ISheet sheet = workbook.CreateSheet("Könyvelt tételek");

                this.CreateHeader(sheet, excelData.Header, isCreateExtendedHeader);

                IDataFormat newDataFormat = workbook.CreateDataFormat();
                ICellStyle dateStyle = workbook.CreateCellStyle();
                dateStyle.DataFormat = newDataFormat.GetFormat("yyyy.MM.dd");

                ICellStyle numberStyle = workbook.CreateCellStyle();
                numberStyle.DataFormat = newDataFormat.GetFormat("0.00");

                int rowIndex = 1;
                foreach (var item in excelData.Transactions)
                {
                    IRow row = sheet.CreateRow(rowIndex);

                    var dateCell = this.CreateStyledCell(row, 0, dateStyle);
                    var numberCell = this.CreateStyledCell(row, 7, numberStyle);

                    dateCell.SetCellValue(item.AccountingDate);
                    row.CreateCell(1).SetCellValue(item.TransactionId);
                    row.CreateCell(2).SetCellValue(item.Type);
                    row.CreateCell(3).SetCellValue(item.Account);
                    row.CreateCell(4).SetCellValue(item.AccountName);
                    row.CreateCell(5).SetCellValue(item.PartnerAccount);
                    row.CreateCell(6).SetCellValue(item.PartnerName);
                    numberCell.SetCellValue(item.Sum);
                    row.CreateCell(8).SetCellValue(item.Currency);
                    row.CreateCell(9).SetCellValue(item.Message);

                    this.watch.PrintDiff($"{rowIndex}/{excelData.Transactions.Count} line(s) created.");
                    rowIndex++;
                }

                workbook.Write(fs);
                this.watch.PrintDiff($"File saved.");
            }
            this.watch.PrintTime($"DONE.\n");
        }

        ICell CreateStyledCell(IRow row, int columnIndex, ICellStyle style)
        {
            ICell cell = row.CreateCell(columnIndex);
            cell.CellStyle = style;
            return cell;
        }

        void CreateHeader(ISheet sheet, List<string> header, bool isCreateExtendedHeader)
        {
            int counter = 0;
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < header.Count; i++)
            {
                row.CreateCell(i).SetCellValue(header[i]);

                if (isCreateExtendedHeader)
                {
                    if (counter == 9)
                    {
                        row.CreateCell(10).SetCellValue("Is omitted?");
                        row.CreateCell(11).SetCellValue("Group id");
                        row.CreateCell(12).SetCellValue("Tag ids");
                        row.CreateCell(13).SetCellValue("Tag group id");
                        return;
                    }
                    counter++;
                }
            }
        }
    }
}
