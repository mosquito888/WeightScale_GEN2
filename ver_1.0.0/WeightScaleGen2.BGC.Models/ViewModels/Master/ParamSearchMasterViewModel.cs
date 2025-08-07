using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Master
{
    public class ParamSearchMasterViewModel : ParamJqueryDataTable
    {
        public string master_type { get; set; }
        public string master_code { get; set; }
        public string master_value1 { get; set; }
        public string master_value2 { get; set; }
        public string master_value3 { get; set; }
        public string master_desc_th { get; set; }
        public string master_desc_en { get; set; }
    }

    public class ParamMasterInfo
    {
        public string master_code { get; set; }
    }
}
