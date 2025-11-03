using BuildNetworkExample;
using Microsoft.Data.Analysis;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using tNav.API;
using tNav.Common;
using TNav = tNav.API;

Console.WriteLine("1 Пример API1.3_HowToBuildNetworkViaAPIServer");
Console.WriteLine("2 Тест на чтение DataFrame");

var variant = Console.ReadLine();
switch (variant)
{
    case "1":
        new LikePython1_3().Run();
        break;
    case "2":
        new TestRead().ReadFrame();
        break;
}
Console.WriteLine("Нажмите любую клавишу для выхода");
Console.ReadKey();