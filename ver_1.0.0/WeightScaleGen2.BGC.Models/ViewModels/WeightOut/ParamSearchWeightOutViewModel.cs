using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightOut
{
    public class ParamSearchWeightOutViewModel : ParamJqueryDataTable
    {
        public string weight_out_no { get; set; }
        public string weight_in_no { get; set; }
    }

    public class ParamWeightOutInfo
    {
        public int id { get; set; }
        public string car_license { get; set; }
    }
}
