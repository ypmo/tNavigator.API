using System;

namespace tNav.API;

public enum ProjectType
{
    /// <summary>
    /// md
    /// </summary>
    MD,
    /// <summary>
    /// gd
    /// </summary>
    GD,
    /// <summary>
    /// nd
    /// </summary>
    ND,
    /// <summary>
    /// rpd
    /// </summary>
    RP,
    /// <summary>
    /// pvtd
    /// </summary>
    PVTD,
    /// <summary>
    /// wd
    /// </summary>
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
