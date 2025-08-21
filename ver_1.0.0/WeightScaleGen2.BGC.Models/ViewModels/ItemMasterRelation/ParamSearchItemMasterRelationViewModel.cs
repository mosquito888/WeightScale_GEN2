using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.ItemMasterRelation
{
    public class ParamSearchItemMasterRelationViewModel : ParamJqueryDataTable
    {
        public string product_code { get; set; }
        public string supplier_code { get; set; }
    }

    public class ParamItemMasterRelationInfo
    {
        public int id { get; set; }
    }
}
