using EwiPraca.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace EwiPraca.Exporters
{
    public class EwiExporter : IEwiExporter
    {
        public MemoryStream ExportEmployees(List<Employee> employees, string fileName)
        {
            var dataTable = GetTable(employees);

            var folderPath = @"C:\MediaFiles";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var excel = new Excel.Application();

            var excelworkBook = excel.Workbooks.Add(Type.Missing);
            // Workk sheet
            var excelSheet = (Excel.Worksheet)excelworkBook.ActiveSheet;
            excelSheet.Name = "Eksport pracowników";
            
            //column headers
            for (int i = 1; i <= dataTable.Columns.Count; i++)
            {
                excelSheet.Cells[1, i] = dataTable.Columns[i - 1].ColumnName;
                excelSheet.Cells.Font.Color = Color.Black;
            }

            int rowcount = 1;

            foreach (DataRow datarow in dataTable.Rows)
            {
                rowcount += 1;
                for (int i = 1; i <= dataTable.Columns.Count; i++)
                {
                    excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();
                }
            }

            // now we resize the columns
            var excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
            excelCellrange.EntireColumn.AutoFit();
            Excel.Borders border = excelCellrange.Borders;
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
            border.Weight = 2d;

            string excelFileName = fileName + ".xlsx";

            var newFullFileName = Path.Combine(folderPath, excelFileName);

            excelworkBook.SaveAs(newFullFileName);
            excelworkBook.Close();
            excel.Quit();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(excelworkBook);

            Marshal.ReleaseComObject(excelCellrange);

            Marshal.ReleaseComObject(excel);

            MemoryStream ms = new MemoryStream();

            using (FileStream fs = File.OpenRead(newFullFileName))
            {
                fs.CopyTo(ms);
            }

            ms.Seek(0, SeekOrigin.Begin);

            File.Delete(newFullFileName);
            return ms;
        }

        private static DataTable GetTable(List<Employee> employees)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Imię", typeof(string));
            table.Columns.Add("Nazwisko", typeof(string));
            table.Columns.Add("PESEL", typeof(string));
            table.Columns.Add("Data urodzenia", typeof(string));
            table.Columns.Add("Stanowisko", typeof(string));
            table.Columns.Add("Ulica", typeof(string));
            table.Columns.Add("Numer", typeof(string));
            table.Columns.Add("Numer lokalu", typeof(string));
            table.Columns.Add("Miasto", typeof(string));
            table.Columns.Add("Kod pocztowy", typeof(string));

            foreach (var employee in employees)
            {
                table.Rows.Add(employee.FirstName, employee.Surname, employee.PESEL, employee.BirthDate.ToShortDateString(), employee.Position?.Description ?? string.Empty,
                    employee.Address.StreetName, employee.Address.StreetNumber, employee.Address.PlaceNumber, employee.Address.City, employee.Address.ZIPCode);
            }

            return table;
        }
    }
}