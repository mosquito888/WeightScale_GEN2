using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightDaily
{
    public class ResultSearchWeightDailyViewModel
    {
        public string weight_in_no { get; set; }
        public string car_license { get; set; }
        public DateTime? weight_in_date { get; set; }
        public decimal weight_in { get; set; }
        public DateTime? weight_out_date { get; set; }
        public decimal before_weight_out { get; set; }
        public decimal weight_receive { get; set; }
        public string user_id { get; set; }
        public string document_ref { get; set; }
        public string weight_out_no { get; set; }
        public string status { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public int supplier_code { get; set; }
        public string supplier_name { get; set; }
        public decimal weight_diff { get; set; }
        public string remark_1 { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
