using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using System.Xml.Linq;
using tNav.Common;
namespace tNav;
public class Connection : IConnection
{
    internal Connection(Process process)
    {
        this.process = process;
    }
    //string tNavigator_API_client_exe = "";
    readonly Process process;

    public IProject CreateProject(string path, CaseType case_type, ProjectType project_type)
    {

        var command = $"create_project (path = \"{path}\", case = \"{case_type.tNavValue()}\", type = \"{project_type.tNavValue()}\")\n";
        process.process_message(command);
        int.TryParse(process.StandardOutput.ReadLine()?.OneToOneToUTF8(), out int project_id);
        var parent_id = ProjectID.Invalid;
        return new Project(
            process, project_id, parent_id, project_type, save_on_close: true);
    }

    public List<string> GetListOfProjects()
    {
        var command = $"get_list_of_projects ()";
        process.process_message(command);
        List<string> projects = [];
        if (!int.TryParse(process.StandardOutput.ReadLine()?.OneToOneToUTF8() ?? "", out int count_str))
        { throw new InvalidCastException("Абракадабра"); }
        for (int i = 0; i < count_str; i++)
        {
            projects.Add((process.StandardOutput.ReadLine()?.OneToOneToUTF8() ?? "").Trim());
        }
        return projects;
    }

    public IProject OpenProject(string path, ProjectType project_type = ProjectType.ModelDesigner, bool save_on_close = false)
    {
        var escaped_path = path.Replace("\\", "\\\\");
        var command = $"open_project (path = \"{escaped_path}\", type=\"{project_type.tNavValue()}\")\n";
        process.process_message(command);
        int.TryParse(process.StandardOutput.ReadLine()?.OneToOneToUTF8(), out int project_id);
        var parent_id = ProjectID.Invalid;
        return new Project(process, project_id, parent_id, project_type, save_on_close);
    }

    public string GetVersionString()
    {
        process.process_message("get_version_string ()\n");
        var version = process.StandardOutput.ReadLine()?.OneToOneToUTF8() ?? "";
        return version;
    }

    void Stop()
    {
        ProcessExtentions.process_message(process, "stop ()\n");
    }

    public void Dispose()
    {
        Stop();
    }
}
