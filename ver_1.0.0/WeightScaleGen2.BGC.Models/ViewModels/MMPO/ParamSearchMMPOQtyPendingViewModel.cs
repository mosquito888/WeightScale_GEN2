using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.MMPO
{
    public class ParamSearchMMPOQtyPendingViewModel : ParamJqueryDataTable
    {
        public string document_po { get; set; }
        public string material_code { get; set; }
        public string line_number { get; set; }
    }
}
