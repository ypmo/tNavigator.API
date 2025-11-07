
namespace tNav.ModelDesignerEx;

public class ModelDesigner : IModelDesigner
{
    IProject project;
    internal ModelDesigner(IProject project)
    {
        this.project = project;
    }

    public void CloseProject() => project.CloseProject();

    public void Dispose()
    {
        project.Dispose();
    }

    public List<string> GetListOfSubProjects(ProjectType type = ProjectType.NenworkDesigner) => project.GetListOfSubProjects(type);

    public IProject GetSubProjectByName(string name, ProjectType type = ProjectType.NenworkDesigner) => project.GetSubProjectByName(name, type);

    public object? RunPyCode(string? file = null, string[]? files = null, string? code = null, bool save = false) => project.RunPyCode(file: file, files: files, code: code, save: save);

    public T? RunPyCode<T>(string? file = null, string[]? files = null, string? code = null, bool save = false) => project.RunPyCode<T>(file: file, files: files, code: code, save: save);

    public void SaveProject() => project.SaveProject();
}
