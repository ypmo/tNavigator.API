using System;
using tNav;
using tNav.ProgectExtentions;

namespace BuildNetworkExample;

public class ZNGKMCalc
{
    FileInfo modelPath;
    FileInfo tNavPath;
    public ZNGKMCalc(FileInfo tNavPath, FileInfo model)
    {
        modelPath = model;
        this.tNavPath = tNavPath;
    }
    public void Run(double? value)
    {
        if (!tNavPath.Exists)
        {
            Console.WriteLine($"Console tNavigator not found at {tNavPath.FullName}!");
            Environment.Exit(1);
        }

        if (!modelPath.Exists)
        {
            Console.WriteLine($"Модель не найдета {modelPath.FullName}");
            Environment.Exit(1);
        }
        value ??= double.Parse(Console.ReadLine() ?? throw new InvalidCastException());
        Console.Write("Opening project...");
     using    var conn = ConnectionFactory.GetConnection(path_to_exe: tNavPath.FullName, license_wait_time_limit__secs: 30);
        using   var MD_proj = conn.OpenProject(path: modelPath.FullName, save_on_close: false);
        Console.WriteLine("Done");

        var nd_projects = MD_proj.GetListOfSubProjects(ProjectType.ND);
        var ND_proj = MD_proj.GetSubProjectByName(nd_projects.First(), ProjectType.ND);

        Console.Write("Requesting licenses...");
        ND_proj.RequestLicenseFeatures(
            LicenseFeature.NetworkDesigner);
        Console.WriteLine("Done");

        Console.Write("Set Sink pressure...");
        var sinks = ND_proj.RunPyCode<Dictionary<string, object>>(code: $"""
sinks=get_sinks()
    sink=sinks["zpa"]

""");
        //sink.set_sink_pressure({value})
        MD_proj.CloseProject();
        Console.WriteLine("Done");

        Console.Write("Calculating surface network...");
        ND_proj.RunPyCode(code: "run_network_model_calculations (result=\"Result\", replace_if_exists=True)");
        Console.WriteLine("Done");





    }
}
