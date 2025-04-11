using DocumentFormat.OpenXml.Spreadsheet;
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

                var value = row[i];
                var type = value.GetType();
                var colvalue = (i, value) switch
                {
                    (_, null) => string.Empty,
                    (_, string v_s) => v_s,
                    (0, int v_i) => v_i.ToString(),
                    (_, int v_i) => ((double)v_i).ToString("N1", CultureInfo.InvariantCulture),
                    (_, double v_d) => (v_d % 1) == 0 ? v_d.ToString("0.0", CultureInfo.InvariantCulture) : v_d.ToString(CultureInfo.InvariantCulture).ToLowerInvariant(),
                    _ => throw new NotImplementedException(),
                };
                colvalue = colvalue?.Replace(",", "_");
                sb.Append($"{splitter}{colvalue}");
                firstStep = false;
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }

  
}
