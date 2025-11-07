namespace tNav.NetworkDesignerEx;

public class NetworkDisigner : INetworkDisigner
{
    IProject project;
    public NetworkDisigner(IProject project)
    {
        this.project = project;
    }
}
