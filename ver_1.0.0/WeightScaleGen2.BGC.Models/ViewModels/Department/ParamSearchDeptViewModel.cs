using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Department
{
    public class ParamSearchDeptViewModel : ParamJqueryDataTable
    {
        public string dept_code { get; set; }
        public string dept_name { get; set; }
    }

    public class ParamDeptInfo
    {
        public string dept_code { get; set; }
    }
}
