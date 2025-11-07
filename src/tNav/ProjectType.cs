using System;

namespace tNav;

public enum ProjectType
{
    /// <summary>
    /// md
    /// </summary>
    ModelDesigner,
    /// <summary>
    /// gd
    /// </summary>
    GD,
    /// <summary>
    /// nd
    /// </summary>
    NenworkDesigner,
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
    WellDesigner,
}


internal static class ProjectTypeEx
{
    public static string tNavValue(this ProjectType type)
    {
        return type switch
        {
            ProjectType.ModelDesigner => "md",
            ProjectType.GD => "gd",
            ProjectType.NenworkDesigner => "nd",
            ProjectType.RP => "rpd",
            ProjectType.PVTD => "pvtd",
            ProjectType.WellDesigner => "wd",
            _ => throw new NotImplementedException()
        };
    }
}
