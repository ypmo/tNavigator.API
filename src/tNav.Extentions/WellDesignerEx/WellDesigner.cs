namespace tNav.WellDesignerEx;

public class WellDesigner : IWellDesigner
{
    IProject project;
    public WellDesigner(IProject project)
    {
        this.project = project;
    }
}
