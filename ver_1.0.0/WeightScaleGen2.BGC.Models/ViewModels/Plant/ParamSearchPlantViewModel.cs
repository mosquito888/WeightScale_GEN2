using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Plant
{
    public class ParamSearchPlantViewModel : ParamJqueryDataTable
    {
        public string comp_code { get; set; }
        public string plant_code { get; set; }
        public string plant_name_th { get; set; }
        public string plant_name_en { get; set; }

    }

    public class ParamPlantInfo
    {
        public string plant_code { get; set; }
    }
}
