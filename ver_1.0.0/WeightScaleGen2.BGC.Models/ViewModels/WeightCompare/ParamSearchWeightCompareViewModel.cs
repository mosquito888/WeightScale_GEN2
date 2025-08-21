using System;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightCompare
{
    public class ParamSearchWeightCompareViewModel : ParamJqueryDataTable
    {
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string item_code { get; set; }
        public int supplier_code { get; set; }
    }
}
