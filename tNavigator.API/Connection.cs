using System;

namespace tNavigator.API;

public class Connection
{

    public Project create_project(string path, CaseType case_type, ProjectType project_type)
    {
        throw new NotImplementedException();
    }

    public List<string> get_list_of_projects()
    {
        throw new NotImplementedException();
    }

    public Project open_project(string path, ProjectType project_type = ProjectType.MD, bool save_on_close = false)
    {
        throw new NotImplementedException();
    }
    public string get_version_string()
    {
         throw new NotImplementedException();
    }

    public void stop()
    {
        throw new NotImplementedException();
    }
}
