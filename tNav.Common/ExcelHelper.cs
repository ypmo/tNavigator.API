using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.Common;
    public static class ExcelHelper
    {
        /// <summary>
        /// КОНВЕРТИРОВАНИЕ xlsx в scv
        /// </summary>
        /// <param name="fileName">Путь к фалу</param>
        /// <param name="sheetNames">Имена загладок для экспорта. Если пусто, то все</param>
        /// <returns>имена и содержимое</returns>
        public static List<(string name, string content)> ExcelToCSV(string fileName, List<string> sheetNames, int firstRow = 0)
        {
            List<(string name, string content)> result = [];
            var workbook = new ClosedXML.Excel.XLWorkbook(fileName);
            foreach (var worksheet in workbook.Worksheets)
            {
                if (!sheetNames.Any() || sheetNames.Contains(worksheet.Name))
                {
                    var sb = new StringBuilder();

                    foreach (var row in worksheet.RowsUsed())
                    {
                        if (row.RowNumber() > firstRow)
                        {
                            var text = string.Join(",", row.Cells(1, row.LastCellUsed().Address.ColumnNumber)
                                .Select(cell => cell.Value.ToString(CultureInfo.InvariantCulture)));
                            sb.AppendLine(text);
                        }
                    }
                    result.Add((worksheet.Name, sb.ToString()));
                }
            }
            return result;
        }
    }

