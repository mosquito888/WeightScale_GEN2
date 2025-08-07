using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightIn
{
    public class ParamSearchWeightInViewModel : ParamJqueryDataTable
    {
        public string weight_in_no { get; set; }
    }

    public class ParamWeightInInfo
    {
        public int id { get; set; }
        public string car_type { get; set; }
        public string car_license { get; set; }
        public string company_code { get; set; }
        public string status { get; set; }
    }
}
