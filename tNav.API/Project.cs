using System.Diagnostics;
using tNav.Common;

namespace tNav.API;

public class Project : IProject
{
    int project_id;
    int parent_id;
    ProjectType type;
    Process process;
    bool save_on_close;

    public Project(
        Process process,
        int project_id = ProjectID.invalid,
        int parent_id = ProjectID.invalid,
        ProjectType project_type = ProjectType.MD,
        bool save_on_close = false)
    {
        this.project_id = project_id;
        this.parent_id = parent_id;
        this.type = project_type;
        this.process = process;
        this.save_on_close = save_on_close;
    }

    public void CloseProject()
    {
        if (save_on_close)
            SaveProject();
        var command = $"close_project (id = \"{project_id}\")\n";
        Processes.process_message(process, command);
    }

    public Project GetSubProjectByName(string name, ProjectType type = ProjectType.ND)
    {
        throw new NotImplementedException();
    }

    public List<string> GetListOfSubProjects(ProjectType type = ProjectType.ND)
    {
        throw new NotImplementedException();
    }

    public object RunPyCode(string? file = null, List<string>? files = null, int? code = null, bool save = false)
    {
        throw new NotImplementedException();
    }

    public void SaveProject()
    {
        var id_to_save = parent_id;
        if (id_to_save == ProjectID.invalid)
            id_to_save = project_id;

        var save_command = $"run_py_code (code = \"save_project ()\", id = \"{id_to_save}\")\n";
        Processes.process_message(process, save_command);
        var result = StreamParser.UnpackString(process.StandardOutput);
    }
}
