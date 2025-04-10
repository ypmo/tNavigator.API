using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
namespace tNav.API;
public class Connection : IConnection
{
    public Connection(Process process)
    {
        this.process = process;
    }
    //string tNavigator_API_client_exe = "";
    readonly Process process;


  
    public IProject CreateProject(string path, CaseType case_type, ProjectType project_type)
    {

        var command = $"create_project (path = \"{path}\", case = \"{case_type.tNavValue()}\", type = \"{project_type.tNavValue()}\")\n";
        Processes.process_message(process, command);
        int.TryParse(Processes.readline(process), out int project_id);
        var parent_id = ProjectID.Invalid;
        return new Project(
            process, project_id, parent_id, project_type, save_on_close: true);
    }

    public List<string> GetListOfProjects()
    {
        throw new NotImplementedException();
    }

    public IProject OpenProject(string path, ProjectType project_type = ProjectType.MD, bool save_on_close = false)
    {
        var command = $"open_project (path = \"{path}\", type=\"{project_type}\")\n";
        Processes.process_message(process, command);
        int.TryParse(Processes.readline(process), out int project_id);
        var parent_id = ProjectID.Invalid;
        return new Project(process, project_id, parent_id, project_type, save_on_close);
    }
    public string GetVersionString()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}
