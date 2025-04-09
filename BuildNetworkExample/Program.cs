#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
using Microsoft.VisualBasic;
using tnav = tNav.API;
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
string tNpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "tNavigator/tNavigator-con");

if (!Path.Exists(tNpath))
{
    Console.WriteLine($"Console tNavigator not found at {Path.GetFullPath(tNpath)}!");
    Environment.Exit(1);
}

if (!Path.Exists("Init_Data"))
{
    Console.WriteLine($"Init_Data folder not found!");
    Environment.Exit(1);
}

List<string> xls_list = ["Init_Data/WD_data.xlsx", "Init_Data/ND_data.xlsx"];

Console.WriteLine("Running script");
Console.Write("Creating and opening snp project...");

var conn = new tnav.Connection(path_to_exe: tNpath, new tnav.ConnectionOptions
{
     
});
var snp_new = conn.CreateProject(path: "SNP/API_BuildND.snp", case_type: tnav.CaseType.MD, project_type: tnav.ProjectType.MD);
snp_new.CloseProject();
var MD_proj = conn.OpenProject(path: "SNP/API_BuildND.snp", save_on_close: false);
Console.WriteLine("Done");