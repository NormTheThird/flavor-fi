using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;


namespace FlavorFi.Common.Converters
{
    public static class Convert
    {

        /// <summary>
        /// Creates a new table from the columns of an Excel file
        /// </summary>
        /// <returns>An empty DataTable with columns for an excel file</returns>
        public static DataTable CreateTableFromExcel()
        {
            try
            {
                // Open Excel file to read column headers
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Open(@"C:\Users\wnorman\Documents\Me\Misc\Test.xlsx");
                Excel.Worksheet worksheet = workbook.Sheets[1];
                Excel.Range range = ((Excel.Worksheet)workbook.Worksheets.get_Item(1)).UsedRange;

                // Create table and populate columns
                DataTable dt = new DataTable();
                for (int i = 1; i <= range.Columns.Count; i++)
                    dt.Columns.Add(((Excel.Range)range.Cells[1, i]).Value2.Trim());

                // Close Excel file and return
                workbook.Close();
                application.Quit();
                Marshal.ReleaseComObject(application);
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }

        }

        /// <summary>
        /// Adds data to the DataTable
        /// </summary>
        /// <param name="_dataTable">The DataTable to populate</param>
        /// <returns>A DataTable with the populated data</returns>
        public static DataTable AddData(DataTable _dataTable)
        {
            try
            {
                DataTable dt = _dataTable.Clone();

                for (int i = 0; i < 10; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < _dataTable.Columns.Count; j++)
                        dr[j] = "Test " + i.ToString() + j.ToString();

                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception)
            {
                return _dataTable;
            }
        }

        /// <summary>
        /// Creates an Excel file from a DataTable
        /// </summary>
        /// <param name="_dataTable">The DataTable to create the excel file from</param>
        /// <param name="_filePath">The path to save the file to</param>
        /// <returns>A bool value if the file creation was successful</returns>
        public static bool CreateExcelFromDataTable(DataTable _dataTable, string _filePath)
        {
            try
            {
                // Open Excel file to read column headers
                DateTime start = DateTime.Now;
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // Write column names to worksheet
                for (int i = 1; i <= _dataTable.Columns.Count; i++)
                    worksheet.Cells[1, i] = _dataTable.Columns[i - 1].ColumnName;

                // Write data to worksheet
                for (int i = 2; i <= _dataTable.Rows.Count + 1; i++)
                    for (int j = 1; j <= _dataTable.Columns.Count; j++)
                        worksheet.Cells[i, j] = _dataTable.Rows[i - 2][j - 1];

                // Save worksheet and close Excel
                DateTime end = DateTime.Now;
                worksheet.SaveAs(_filePath);
                workbook.Close();
                application.Quit();
                Marshal.ReleaseComObject(application);

                // Kill all Excel processes
                foreach (System.Diagnostics.Process clsProcess in System.Diagnostics.Process.GetProcesses())
                    if (clsProcess.ProcessName.Equals("EXCEL"))
                        clsProcess.Kill();

                // Return success
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a delimited file from a DataTable
        /// </summary>
        /// <param name="_dataTable">The DataTable to create the delimited file from</param>
        /// <param name="_filePath">The path to save the file to</param>
        /// <param name="_delimitedValue">The string value used to seperate the file</param>
        /// <returns>A bool value if the file creation was successful</returns>
        public static bool CreateDelimitedFileFromDataTable(DataTable _dataTable, string _filePath, string _delimitedValue)
        {
            try
            {
                // Loop through each column header
                StringBuilder stringBuilder = new StringBuilder();
                foreach (DataColumn headerColumn in _dataTable.Columns)
                {
                    stringBuilder.Append(headerColumn.ColumnName.ToString() + _delimitedValue.Trim());
                    if (headerColumn.Ordinal == _dataTable.Columns.Count - 1)
                        stringBuilder.Append("~|");
                }

                // Loop through each datarow
                foreach (DataRow dataRow in _dataTable.Rows)
                {
                    // Loop through each column in datarow
                    foreach (DataColumn dataColumn in _dataTable.Columns)
                    {
                        stringBuilder.Append( dataRow[dataColumn].ToString() + _delimitedValue.Trim());
                        if (dataColumn.Ordinal == _dataTable.Columns.Count - 1)
                            stringBuilder.Append("~|");
                           
                    }
                }

                // Save the file to the path provided
                FileStream file = new FileStream(_filePath, FileMode.CreateNew, FileAccess.ReadWrite);
                StreamWriter writer = new StreamWriter(file, Encoding.UTF8);
                writer.Write(stringBuilder.ToString());
                writer.Flush();
                writer.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
