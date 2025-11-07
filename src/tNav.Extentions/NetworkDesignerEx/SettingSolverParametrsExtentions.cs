using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tNav.NetworkDesignerEx;

public static class SettingSolverParametrsExtentions
{
    public static void SettingSolverParametr(this IProject project, SolverParameters parameters)
    {
        _ = project.RunPyCode(code: $"""
nd_settings_solver_parameters (
  temperature_options_widget={parameters.Temperature_options_widget}, 
  use_temperature_equation = True, use_heat_balance_equation = True, 
  use_iterative_method = True, initial_approximation_options_widget = True, 
  use_initial_approximation = False, use_directed_graph_for_initial_rate_approximation = True, 
  use_constraints_for_initial_rate_approximation = False, 
  wells_uterations_chop_settings_widget = True, wells_chop_coefficient = 0.5, 
  use_limit_chop_well_solution = False, chop_well_solution_max_count = 50, 
  newton_iterations_widget = True, use_newton_step_mult = False, 
  newt_step_mult_start_iteration = 2, verification_widget = True, 
  enable_verification = True, linear_solver_settings_widget = True, 
  solver_type = "iterative", newton_iter = 50, solver_max_it = 400, 
  tolerance_widget = True, newton_rhs_tol = 0.001, newton_diff_tol = 1e-8, 
  double_newton_relaxation = 1, use_separate_tol_for_object_pressure_rhs = False, 
  object_pressure_equations_tol = 0.001, differentiation_widget = True, 
  enable_automatic_differentiation = False)
""");
    }
}
public class SolverParameters
{
    public bool Temperature_options_widget { get; set; } = true;
    public bool Use_temperature_equation { get; set; } = true;
    public bool Use_heat_balance_equation { get; set; } = true;
    public bool Use_iterative_method { get; set; } = true;
    public bool Initial_approximation_options_widget { get; set; } = true;
    public bool Use_initial_approximation { get; set; } = false;
    public bool Use_directed_graph_for_initial_rate_approximation { get; set; } = true;
    public bool Use_constraints_for_initial_rate_approximation { get; set; } = false;
    public bool Wells_uterations_chop_settings_widget { get; set; } = true;
    public double Wells_chop_coefficient { get; set; } = 0.5;
    public bool Use_limit_chop_well_solution { get; set; } = false;
    public int Chop_well_solution_max_count { get; set; } = 50;
    public bool Newton_iterations_widget { get; set; } = true;
    public bool Use_newton_step_mult { get; set; } = false;
    public int Newt_step_mult_start_iteration { get; set; } = 2;
    public bool Verification_widget { get; set; } = true;
    public bool Enable_verification { get; set; } = true;
    public bool Linear_solver_settings_widget { get; set; } = true;
    public string Solver_type { get; set; } = "iterative";
    public int Newton_iter { get; set; } = 50;
    public int Solver_max_it { get; set; } = 400;
    public bool Tolerance_widget { get; set; } = true;
    public double Newton_rhs_tol { get; set; } = 0.001;
    public double Newton_diff_tol { get; set; } = 1e-8;
    public int Double_newton_relaxation { get; set; } = 1;
    public bool Use_separate_tol_for_object_pressure_rhs { get; set; } = false;
    public double Object_pressure_equations_tol { get; set; } = 0.001;
    public bool Differentiation_widget { get; set; } = true;
    public bool Enable_automatic_differentiation { get; set; } = false;
}
