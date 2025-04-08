using System;

namespace tNavigator.API;

public interface IConnection
{
    Project create_project(string path, CaseType case_type, ProjectType project_type);
    List<string> get_list_of_projects();
    Project open_project(string path, ProjectType project_type = ProjectType.MD, bool save_on_close = false);
    string get_version_string();
    void stop();
}
