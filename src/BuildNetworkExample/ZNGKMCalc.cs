using System;
using Microsoft.Data.Analysis;
using tNav;
using tNav.Common;
using tNav.ModelDesignerEx;
using tNav.NetworkDesignerEx;
using tNav.ProjectExtentions;

namespace BuildNetworkExample;

public class ZNGKMCalc
{
    FileInfo modelPath;
    FileInfo tNavPath;
    string time_format = @"'year='yyyy',month='%M', day='%d";
     List<DateTime> timestamps = [new DateTime(2025,10,22)];
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
        if (value == null)
        {
            Console.WriteLine("Укажите выходное давление...");
            value ??= double.Parse(Console.ReadLine() ?? throw new InvalidCastException());
        }

        Console.Write("Opening project...");
        using var conn = ConnectionFactory.GetConnection(path_to_exe: tNavPath.FullName, license_wait_time_limit__secs: 30);
        var MD_proj = conn.OpenProject(path: modelPath.FullName, save_on_close: false);
        Console.WriteLine("Done");

        var nd_projects = MD_proj.GetListOfSubProjects(ProjectType.NenworkDesigner);
        var ND_proj = MD_proj.GetSubProjectByName(nd_projects.First(), ProjectType.NenworkDesigner);

        Console.Write("Requesting licenses...");
        ND_proj.RequestLicenseFeatures(
            LicenseFeature.NetworkDesigner);
        Console.WriteLine("Done");



        Console.Write("Set Sink pressure...");
        ND_proj.RunPyCode(code: $"""
object_parameters_change (event_date=datetime (year=2025,
      month=10,
      day=22,
      hour=0,
      minute=0,
      second=0,
      microsecond=0),
      index=find_nd_object (name="zpa",
      type="sink"),
      event_type="sink_pressure",
      value={value})
""");
        Console.WriteLine("Done");

        Console.Write("Calculating surface network...");
        ND_proj.RunPyCode(code: "run_network_model_calculations (result=\"Result\", replace_if_exists=True)");
        Console.WriteLine("Done");

   Console.Write("Результаты расчета труб...");
        var pipes = ND_proj.GetPipeResults(timestamps[0], "Result");
        foreach (var p in pipes)
        {
            Console.WriteLine($"{p.Index}\t{p.Name}\t{p.Length}\t{p.Pressure}\t{p.Temperature}");
        }
        Console.Write("Результаты расчета скважин...");
        var wells = ND_proj.GetWellResults(timestamps[0], "Result");
        foreach (var p in wells)
        {
            Console.WriteLine($"{p.Index}\t{p.Name}\t{p.Length}\t{p.Pressure}\t{p.Temperature}");
        }

        MD_proj.CloseProject();
        Console.WriteLine("Done");
    }
}
