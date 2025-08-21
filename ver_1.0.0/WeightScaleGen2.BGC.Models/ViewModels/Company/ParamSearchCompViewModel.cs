using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Company
{
    public class ParamSearchCompViewModel : ParamJqueryDataTable
    {
        public string comp_code { get; set; }
        public string comp_name_th { get; set; }
        public string comp_name_en { get; set; }

    }

    public class ParamCompInfo
    {
        public string comp_code { get; set; }
    }
}
