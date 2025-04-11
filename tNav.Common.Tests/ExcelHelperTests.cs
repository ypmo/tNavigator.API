using System;
using Microsoft.Data.Analysis;

namespace tNav.Common.Tests;

public class ExcelHelperTests
{
    [Fact]
    public void CanReadToDataFrame()
    {
        System.Globalization.CultureInfo.CurrentCulture=System.Globalization.CultureInfo.InvariantCulture;
        List<(string name , string content)> csvs = ExcelHelper.ExcelToCSV("ExcelData.xlsx", ["Casing"], 1);
        Assert.True(csvs.Count==1);
        var name=csvs[0].name;
        var content=csvs[0].content;
        Assert.Equal("Casing",name);
        Assert.NotNull(content);
        DataFrame  df=DataFrame.LoadCsvFromString(content);
    }

}
