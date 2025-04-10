using System;
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

    internal Project(
        Process process,
        int project_id = ProjectID.Invalid,
        int parent_id = ProjectID.Invalid,
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
        process.process_message(command);
    }


    IProject IProject.GetSubProjectByName(string name, ProjectType type)
    {
        var command = $"get_subproject_by_name (id = \"{project_id}\", name = \"{name}\", type = \"{type.tNavValue()}\")\n";
        process.process_message(command);
        if (!int.TryParse(process.StandardOutput.ReadLine()!.Trim(), out int new_id))
        {
            throw new InvalidCastException("Не удалось получить идентификатор проект");
        }

        return new Project(
            process: process,
            project_id: new_id,
            parent_id: project_id,
            project_type: type);
    }

    public List<string> GetListOfSubProjects(ProjectType type = ProjectType.ND)
    {
        var command = $"get_list_of_subprojects (id = \"{project_id}\", type = \"{type.tNavValue()}\")\n";
        process.process_message(command);
        List<string> subprojects = [];
        if (!int.TryParse(process.StandardOutput.ReadLine() ?? "", out int count_str))
        { throw new InvalidCastException("Не удалось получить количество проектов"); }
        for (int i = 0; i < count_str; i++)
        {
            subprojects.Add(process.StandardOutput.ReadLine()!.Trim());
        }
        return subprojects;
    }

  

    public object? RunPyCode(string? file = null, string[]? files = null, string? code = null, bool save = false)
    {
        var code_from_file = "";
        if (file != null)
            code_from_file = File.ReadAllText(file);
        if (files != null)
            foreach (var script_with_code in files)
            {
                code_from_file += File.ReadAllText(script_with_code);
                code_from_file += "\n";
            };

        if (code_from_file == "" && code == null)
        {
            throw new ArgumentNullException("run_py_code needs at least one argument");
        };

        var res_code = code_from_file;

        if (code != null)
            res_code += "\n" + code;

        res_code = res_code.Replace("\\", "\\\\");
        res_code = res_code.Replace("\"", "\\\"");
        var command = $"run_py_code (code = \"{res_code}\", id = \"{project_id}\")\n";

        process.process_message(command);
        var ret_value = StreamParser.Unpack_data(process.StandardOutput);

        if (save)
            SaveProject();
        return ret_value;
    }
    void SaveProject()
    {
        var id_to_save = parent_id;
        if (id_to_save == ProjectID.Invalid)
            id_to_save = project_id;

        var save_command = $"run_py_code (code = \"save_project ()\", id = \"{id_to_save}\")\n";
        process.process_message(save_command);
        var result = StreamParser.UnpackString(process.StandardOutput);
    }

    public void Dispose()
    {
        if (parent_id == ProjectID.Invalid)
        {
            CloseProject();
        }
    }



}
