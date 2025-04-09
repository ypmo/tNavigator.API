using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.Common;

public static class Utils
{
    public static string DataTableToCSV(DataTable table)
    {
        StringBuilder sb = new StringBuilder();
        bool firstStep = true;
        for (int i = 0; i < table.Columns.Count; i++)
        {
            string splitter = firstStep ? "" : ",";

            var colname = table.Columns[i].ColumnName;
            if (colname == "NONAME")
            {
                colname = string.Empty;
            }
            colname = colname.Replace(",", "_");
            sb.Append(splitter + colname);
            firstStep = false;
        }
        sb.Append('\n');
        foreach (DataRow row in table.Rows)
        {
            firstStep = true;
            for (var i = 0; i < table.Columns.Count; i++)
            {
                string splitter = firstStep ? "" : ",";
                var colvalue = Convert.ToString(row[i], CultureInfo.InvariantCulture);
                colvalue = colvalue?.Replace(",", "_");
                sb.Append($"{splitter}{colvalue}");
                firstStep = false;
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }
}
