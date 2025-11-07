using System.Globalization;
using System.Text;

namespace tNav.Common;

public static class ExcelHelper
{
    /// <summary>
    /// КОНВЕРТИРОВАНИЕ xlsx в scv
    /// </summary>
    /// <param name="fileName">Путь к фалу</param>
    /// <param name="sheetNames">Имена загладок для экспорта. Если пусто, то все</param>
    /// <returns>имена и содержимое</returns>
    public static List<(string name, string content)> ExcelToCSV(string fileName, List<string> sheetNames , int firstRow = 0)
    {
        List<(string name, string content)> result = [];
        var workbook = new ClosedXML.Excel.XLWorkbook(fileName);
        foreach (var worksheet in workbook.Worksheets)
        {
            if (!sheetNames.Any() || sheetNames.Contains(worksheet.Name))
            {
                var sb = new StringBuilder();
                bool firstRead = true;
                int ncols = 0;
                foreach (var row in worksheet.RowsUsed())
                {
                    if (row.RowNumber() > firstRow)
                    {
                        if (firstRead)
                        {
                            ncols = row.LastCellUsed().Address.ColumnNumber;
                        }
                        var text = string.Join(",", row.Cells(1, ncols)
                            .Select(cell => cell.Value.ToString(CultureInfo.InvariantCulture).Trim()));
                        sb.AppendLine(text);

                        firstRead = false;
                    }
                }
                result.Add((worksheet.Name, sb.ToString()));
            }
        }
        return result;
    }
}

