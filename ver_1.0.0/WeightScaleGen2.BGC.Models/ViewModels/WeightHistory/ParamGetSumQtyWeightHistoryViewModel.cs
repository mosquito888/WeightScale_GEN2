using System;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightHistory
{
    public class ParamGetSumQtyWeightHistoryViewModel : ParamJqueryDataTable
    {
        public string document_po { get; set; }
        public string item_code { get; set; }
        public string line_number { get; set; }
        public DateTime date { get; set; }
    }
}
