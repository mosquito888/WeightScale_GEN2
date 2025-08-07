using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.ItemMaster
{
    public class ParamSearchItemMasterViewModel : ParamJqueryDataTable
    {
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string status { get; set; }
    }

    public class ParamItemMasterInfo
    {
        public string product_code { get; set; }
    }
}
