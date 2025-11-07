namespace tNav.NetworkDesignerEx;

public enum NetworkModelObjectType
{
    Well,
    Pipe
}

internal static class NetworkModelObjectTypeExtentions
{
    public static string Name(this NetworkModelObjectType type) => type switch
    {
        NetworkModelObjectType.Well => "well",
        NetworkModelObjectType.Pipe => "pipe",
        _ => throw new NotImplementedException(),
    };
}
