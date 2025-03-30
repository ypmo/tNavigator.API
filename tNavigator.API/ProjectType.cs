using System;

namespace tNavigator.API;

public enum ProjectType
{
    MD,
    GD,
    ND,
    RP,
    PVTD,
    WD,
}
public static class ProjectTypeEx
{
    public static string tNavValue(this ProjectType type)
    {
        return (type) switch
        {
            ProjectType.MD => "md",
            ProjectType.GD => "gd",
            ProjectType.ND => "nd",
            ProjectType.RP => "rpd",
            ProjectType.PVTD => "pvtd",
            ProjectType.WD => "wd",
            _ => throw new NotImplementedException()
        };
    }
}
