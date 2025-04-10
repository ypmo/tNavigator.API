using System;

namespace tNav.API;

public interface IConnection
{
    IProject CreateProject(string path, CaseType case_type, ProjectType project_type);

    List<string> GetListOfProjects();

    IProject OpenProject(string path, ProjectType project_type = ProjectType.MD, bool save_on_close = false);

    string GetVersionString();

    void Stop();
}
