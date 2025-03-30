namespace tNavigator.API;

public class Project
{
    public void close_project()
    {
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
}
