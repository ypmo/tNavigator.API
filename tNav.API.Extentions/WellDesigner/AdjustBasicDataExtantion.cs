using System;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;

namespace tNav.WellDesigner;

public static class AdjustBasicDataExtantion
{
    public static void AdjustBasicData(this IProject project, AdjustBasicDataOptions opt)
    {
        var command = $"""
well_designer_adjust_basic_data (
    name = "{opt.Name}",
    group_name = "{opt.group_name}",
    object = "{opt.Object}",
    well_type = "{opt.well_type.Name()}",
    current_vfp = "{opt.current_vfp}",
    preferred_phase = "{opt.preferred_phase}",
    reference_depth_mode = "{opt.reference_depth_mode}",
    user_tvd = {opt.user_tvd}, 
    inflow_equation = "{opt.inflow_equation}",
    instructions = "{opt.instructions}", 
    density_type = "{opt.density_type}",
    drainage_radius = {opt.drainage_radius},
    crossflow_ability = {opt.crossflow_ability},
    use_fluid_esp_heating = {opt.use_fluid_esp_heating},
    max_deviation_angle = {opt.max_deviation_angle},
    use_segment_model = {opt.use_segment_model}, 
    flow_model = {opt.flow_model},
    suppress_annular_segments = {opt.suppress_annular_segments},
    use_segment_params = {opt.use_segment_params},
    min_segment_length = {opt.min_segment_length},
    max_segment_length = {opt.max_segment_length},
    use_thermal_parameters = {opt.use_thermal_parameters},
    thickness = {opt.thickness},
    thermal_conductivity = {opt.thermal_conductivity},
    link_segment_nodes = {opt.link_segment_nodes},
    well_head_x = {opt.well_head_x},
    well_head_y = {opt.well_head_y},
    well_head_z = {opt.well_head_z},
    sc_pressure = {opt.sc_pressure},
    sc_temperature = {opt.sc_temperature},
    use_concentric_tubings = {opt.use_concentric_tubings},
    use_segment_graph = {opt.use_segment_graph},
    use_bottomhole_depth_unification = {opt.use_bottomhole_depth_unification})
""";
        _ = project.RunPyCode(code: command);
    }
}
public class AdjustBasicDataOptions
{
    public required string Name { get; set; }
    public string group_name { get; set; } = "";
    public string Object { get; set; } = "well";
    public required well_type well_type { get; set; }
    public string current_vfp { get; set; } = "";
    public string preferred_phase { get; set; } = "1*";
    public string reference_depth_mode { get; set; } = "auto";
    public int user_tvd { get; set; } = 0;
    public string inflow_equation { get; set; } = "STD";
    public string instructions { get; set; } = "SHUT";
    public string density_type { get; set; } = "SEG";
    public int drainage_radius { get; set; } = 0;
    public bool crossflow_ability { get; set; } = true;
    public bool use_fluid_esp_heating { get; set; } = false;
    public int max_deviation_angle { get; set; } = 5;
    public bool use_segment_model { get; set; } = false;
    public bool flow_model = false;
    public bool suppress_annular_segments { get; set; } = false;
    public bool use_segment_params { get; set; } = false;
    public int min_segment_length { get; set; } = 0;
    public int max_segment_length { get; set; } = 1000;
    public bool use_thermal_parameters { get; set; } = false;
    public int thickness { get; set; } = 0;
    public int thermal_conductivity { get; set; } = 0;
    public bool link_segment_nodes { get; set; } = false;
    public double well_head_x { get; set; } = 0;
    public double well_head_y { get; set; } = 0;
    public double well_head_z { get; set; } = 0;
    public double sc_pressure { get; set; } = 0;
    public double sc_temperature { get; set; } = 0;
    public bool use_concentric_tubings { get; set; } = false;
    public bool use_segment_graph { get; set; }
    public bool use_bottomhole_depth_unification { get; set; } = false;
}

public enum well_type
{
    producer
}
internal static class well_typeExtention
{
    public static string Name(this well_type well_Type) => (well_Type) switch
    {
        well_type.producer => "producer",
        _ => throw new NotImplementedException(),
    };
}