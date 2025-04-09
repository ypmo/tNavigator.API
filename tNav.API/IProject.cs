using System;

namespace tNav.API;

public interface IProject
{      
    void SaveProject();

    void CloseProject();

    Project GetSubProjectByName(string name, ProjectType type = ProjectType.ND);

    List<string> GetListOfSubProjects(ProjectType type = ProjectType.ND);

    object RunPyCode(string? file = null, List<string>? files = null, int? code = null, bool save = false);
}
