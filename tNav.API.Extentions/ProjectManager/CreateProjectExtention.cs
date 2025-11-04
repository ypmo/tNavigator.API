using System;
namespace tNav.ProjectManager;

public static class CreateProjectExtention
{
    public static void CreateProject(this IProject project, params (CreateProjectType type, string name)[] values)
    {
        Func<(CreateProjectType type, string name), string> func = (t) =>
        $"{{\"project_type\" : \"{t.type.Name()}\", \"project_name\" : \"{t.name}\"}}";
        var command = $"""
project_manager_create_project ( projects_table = [ 
{string.Join(", ", values.Select(t=>func(t)))}])
""";
        _ = project.RunPyCode(code: command);
    }

}

public enum CreateProjectType
{
    vfp_project,
    nd_project
}

public static class ProjectTypeExtention
{
    public static string Name(this CreateProjectType projectType) => (projectType) switch
    {
        CreateProjectType.vfp_project => "vfp_project",
        CreateProjectType.nd_project => "nd_project",
        _ => throw new NotImplementedException(),
    };
}