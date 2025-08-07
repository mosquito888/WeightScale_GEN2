using System;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightHistory
{
    public class ParamSearchWeightHistoryViewModel : ParamJqueryDataTable
    {
        public DateTime? date { get; set; }
        public string po_number { get; set; }
    }

    public class ParamWeightHistoryInfo
    {
        public int id { get; set; }
    }
}
