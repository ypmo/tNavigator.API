using System;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;

namespace tNav.ProgectExtentions;

public static class PvtImportElFoFormatExtention
{
    public static void PvtImportElFoFormat(this IProject project,
        PvtImportElFoFormatOptions opt)
    {
        var command = $"""
pvt_import_e1_format (
 file_name = "{opt.FileName}",
 region_count = {opt.RegionCount},
 units = "{opt.Units.Name()}",
 clear_tables = {opt.ClearTables}
)
""";
        _ = project.RunPyCode(code: command);
    }

    public class PvtImportElFoFormatOptions
    {
        public required string FileName { get; set; }
        public required int RegionCount { get; set; }
        public Units Units { get; set; } = Units.Metric;
        public bool ClearTables { get; set; } 
    }
}





