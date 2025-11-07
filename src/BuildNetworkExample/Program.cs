using BuildNetworkExample;
using Microsoft.Data.Analysis;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Data;
using System.Linq;


System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;


Option<FileInfo> tNavOption = new("--tNav")
{
    Description = "Путь к приложению 'консоль тНавигатор'",
    Recursive=true,
    Required=true
};

Option<FileInfo> projectOption = new("--project")
{
    Description = "Путь к проекту тНавигатор",
};

Option<FileInfo> dataFrameOption = new("--dataframe")
{
    Description = "Файл",
    DefaultValueFactory = parseResult => new FileInfo(Path.Combine(AppContext.BaseDirectory, "dataframe.txt")),
};

Option<double?> valueOption = new("--value")
{
    Description = "Value",
    DefaultValueFactory = parseResult => 25d
};

RootCommand rootCommand = new("Example tNavigator Console tool");
Command runCommand = new("run", "Запусть проект тНавигатор")
{
    tNavOption,
};
rootCommand.Subcommands.Add(runCommand);

Command exampleCommand = new("example", "Выполнить тестовый пример")
{
};
runCommand.Subcommands.Add(exampleCommand);

Command zpaCommand = new("zpa", "Выполнить расчет до ЗПА")
 {
    projectOption,
    valueOption
};
runCommand.Subcommands.Add(zpaCommand);

Command testCommand = new("test", "Выполнить чтение в DataFrame")
{
     dataFrameOption,
};
rootCommand.Subcommands.Add(testCommand);

exampleCommand.SetAction(parseResult =>
{
    new LikePython1_3_NewAPI().Run(parseResult.GetValue(tNavOption)!);
    return 0;
});
testCommand.SetAction(parseResult =>
{
    new TestRead().ReadFrame(parseResult.GetValue(dataFrameOption)!);
    return 0;
});

zpaCommand.SetAction(parseResult =>
{
    new ZNGKMCalc(tNavPath: parseResult.GetValue(tNavOption)!, model: parseResult.GetValue(projectOption)!)
        .Run(parseResult.GetValue(valueOption));
    return 0;
});

ParseResult parseResult = rootCommand.Parse(args);
return parseResult.Invoke();