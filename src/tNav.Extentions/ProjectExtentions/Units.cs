namespace tNav.ProjectExtentions;

public enum Units
{
    Metric,
}

public static class UnitsExtention
{
    public static string Name(this Units unit) => (unit) switch
    {
        Units.Metric => "METRIC",
        _ => throw new NotImplementedException(),
    };
}

