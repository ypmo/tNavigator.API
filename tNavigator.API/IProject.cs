using System;

namespace tNavigator.API;

public interface IProject
{      
    void save_project();
    void close_project();
    Project get_subproject_by_name(string name, ProjectType type = ProjectType.ND);
    List<string> get_list_of_subprojects(ProjectType type = ProjectType.ND);
    object run_py_code(string? file = null, List<string>? files = null, int? code = null, bool save = false);
}
