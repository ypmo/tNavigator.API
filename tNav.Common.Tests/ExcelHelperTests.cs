using System;
using Microsoft.Data.Analysis;

namespace tNav.Common.Tests;

public class ExcelHelperTests
{
    [Fact]
    public void CanReadToDataFrame()
    {
        System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        List<(string name, string content)> csvs = ExcelHelper.ExcelToCSV("ExcelData.xlsx", ["Casing"], 1);
        Assert.True(csvs.Count == 1);
        var name = csvs[0].name;
        var content = csvs[0].content;
        Assert.Equal("Casing", name);
        Assert.NotNull(content);
        DataFrame df = DataFrame.LoadCsvFromString(content);
    }

    [Fact]
    public void ReadNullToDataFrame()
    {
        System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        List<(string name, string content)> csvs = ExcelHelper.ExcelToCSV("ExcelData.xlsx", ["VFP Correlation Plotting Points"], 1);
        var name = csvs[0].name;
        var content = csvs[0].content;
        Assert.NotNull(content);
        DataFrame df = DataFrame.LoadCsvFromString(content);
    }


    [Fact]
    public void ReadDateTimeDataFrame()
    {
        System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        List<(string name, string content)> csvs = ExcelHelper.ExcelToCSV("ExcelData.xlsx", ["Chokes"], 1);
        var name = csvs[0].name;
        var content = csvs[0].content;
        Assert.NotNull(content);
        DataFrame df = DataFrame.LoadCsvFromString(content);
        foreach(var row in df.Rows)
        {
            var dt=row["time_step"];
            Assert.IsType<DateTime>(dt);
        }
        var value=df.Rows[0]["time_step"];
        Assert.True(value is DateTime);
        Assert.Equal(value, new DateTime(2024,03,14));
    }
}
