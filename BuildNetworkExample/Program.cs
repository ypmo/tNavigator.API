using BuildNetworkExample;
using Microsoft.Data.Analysis;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
string tNavPath = "/home/sergey/tNavigator/v25.2-4329-g14fa64fce903/tNavigator-con";

Console.WriteLine($"Текущая директория {Directory.GetCurrentDirectory()}");
 
Console.WriteLine("1 Пример API1.3_HowToBuildNetworkViaAPIServer");
Console.WriteLine("2 Пример API1.3_HowToBuildNetworkViaAPIServer new API");
Console.WriteLine("3 Тест на чтение DataFrame");

var variant = Console.ReadLine();
switch (variant)
{
    case "1":
        new LikePython1_3().Run();
        break;
    case "2":
        new LikePython1_3_NewAPI().Run(tNavPath);
        break;
    case "3":
        new TestRead().ReadFrame();
        break;
}
Console.WriteLine("Нажмите любую клавишу для выхода");
Console.ReadKey();