using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.UOMConversion
{
    public class ParamSearchUOMConversionViewModel : ParamJqueryDataTable
    {
        public string material_code { get; set; }
        public string uom { get; set; }
    }
}
