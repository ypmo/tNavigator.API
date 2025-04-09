using System.Diagnostics;
using tNav.Common;

namespace tNav.API;

public class Project : NavBase
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
    public void close_project()
    {
        if (save_on_close)
            save_project();
        var command = $"close_project (id = \"{project_id}\")\n";
        process_message(process, command);
    }
    public Project get_subproject_by_name(string name, ProjectType type = ProjectType.ND)
    {
        throw new NotImplementedException();
    }
    public List<string> get_list_of_subprojects(ProjectType type = ProjectType.ND)
    {
        throw new NotImplementedException();
    }

    public object run_py_code(string? file = null, List<string>? files = null, int? code = null, bool save = false)
    {
        throw new NotImplementedException();
    }

    void save_project()
    {
        var id_to_save = parent_id;
        if (id_to_save == ProjectID.invalid)
            id_to_save = project_id;

        var save_command = $"run_py_code (code = \"save_project ()\", id = \"{id_to_save}\")\n";
        process_message(process, save_command);
        var result = StreamParser.UnpackString(process.StandardOutput);
    }

}
