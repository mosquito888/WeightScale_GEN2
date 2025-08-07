using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightCompare
{
    public class ResultSearchWeightCompareViewModel
    {
        public DateTime? date { get; set; }
        public string car_license { get; set; }
        public string document_ref { get; set; }
        public decimal weight_out { get; set; }
        public decimal weight_cal { get; set; }
        public decimal weight_receive { get; set; }
        public decimal weight_by_supplier { get; set; }
        public decimal weight_diff { get; set; }
        public decimal weight_percent { get; set; }
        public string remark_1 { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
