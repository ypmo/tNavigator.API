using System;

namespace tNav.ProjectExtentions;

public static class LicenseRequest
{
    public static void RequestLicenseFeatures(this IProject project, params LicenseFeature[] licenseFeatures)
    {
        Func<LicenseFeature, string> func = (f) => $"{{\"feature\" : \"{f.Name()}\"}}";
        var command = $"""
request_license_features (requested_features=[
{string.Join(",", licenseFeatures.Select(t => func(t)))}])
""";
        _ = project.RunPyCode(code: command);
    }
}
public enum LicenseFeature
{
    ModelDesiner,
    NetworkDesigner,
    WellDesigne,
    PvtDesigner,
}
internal static class LicenseFeatureExtention
{
    public static string Name(this LicenseFeature featuter) => (featuter) switch
    {
        LicenseFeature.ModelDesiner => "FEAT_MODEL_DESIGNER",
        LicenseFeature.NetworkDesigner => "FEAT_NETWORK_DESIGNER",
        LicenseFeature.WellDesigne => "FEAT_WELL_DESIGNER",
        LicenseFeature.PvtDesigner => "FEAT_PVT_DESIGNER",
        _ => throw new NotImplementedException(),
    };
}