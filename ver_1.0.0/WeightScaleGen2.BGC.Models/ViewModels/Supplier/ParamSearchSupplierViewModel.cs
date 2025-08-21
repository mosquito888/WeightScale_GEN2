using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Supplier
{
    public class ParamSearchSupplierViewModel : ParamJqueryDataTable
    {
        public string supplier_code { get; set; }
        public string supplier_name { get; set; }
        public string status { get; set; }
    }

    public class ParamSupplierInfo
    {
        public string supplier_code { get; set; }
    }
}
