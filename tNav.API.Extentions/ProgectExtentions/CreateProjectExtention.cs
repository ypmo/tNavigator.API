using System;
namespace tNav.ProgectExtentions;

public static class CreateProjectExtention
{
    public static void CreateProject(IProject project, params (ProjectType type, string name)[] values)
    {
        Func<(ProjectType type, string name), string> func = (t) =>
        $"{{\"project_type\" : \"{t.type.Name()}\", \"project_name\" : \"{t.name}\"}}";
        var command = $"""
project_manager_create_project ( projects_table = [ 
{string.Join(", ", values.Select(t=>func(t)))}])
""";
        _ = project.RunPyCode(code: command);
    }

}

public enum ProjectType
{
    vfp_project,
    nd_project
}

public static class ProjectTypeExtention
{
    public static string Name(this ProjectType projectType) => (projectType) switch
    {
        ProjectType.vfp_project => "vfp_project",
        ProjectType.nd_project => "nd_project",
        _ => throw new NotImplementedException(),
    };
}