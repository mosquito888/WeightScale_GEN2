using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Employee
{
    public class ParamSearchEmpViewModel : ParamJqueryDataTable
    {
        public string emp_code { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }

    public class ParamEmpInfo
    {
        public string emp_code { get; set; }
    }
}
