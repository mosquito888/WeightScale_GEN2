using System;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class WeightCompareData
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
    }
}
