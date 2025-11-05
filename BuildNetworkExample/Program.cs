using BuildNetworkExample;
using Microsoft.Data.Analysis;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Data;
using System.Linq;


System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;


Option<FileInfo> tNavPathOption = new("--tNav")
{
    Description = "Путь к приложению 'консоль тНавигатор'"
};


Option<string?> actionOption = new("--action")
{
    Description = "автоматическое выполнение пункта меню",
    DefaultValueFactory = parseResult => null
};

Option<FileInfo> fileNameOption = new("--file")
{
    Description = "FileName",
    DefaultValueFactory = parseResult => new FileInfo(Path.Combine(AppContext.BaseDirectory, "dataframe.txt"))
};

var rootCommand = new RootCommand("Example tNavigator Console tool");
Command dialogCommand = new("runExample", "Выполнить тестовый пример")
 {
    tNavPathOption,
    actionOption
 };
Command testDataFrameCommand = new("testDataFrame", "Выполнить чтение в DataFrame")
 {
    fileNameOption,
 };

rootCommand.Subcommands.Add(dialogCommand);
rootCommand.Subcommands.Add(testDataFrameCommand);

dialogCommand.SetAction(parseResult =>
{
    new Dialog(tNavPath: parseResult.GetValue(tNavPathOption)).Run(parseResult.GetValue(actionOption));
    return 0;
});
testDataFrameCommand.SetAction(parseResult =>
{

});

ParseResult parseResult = rootCommand.Parse(args);
return parseResult.Invoke();