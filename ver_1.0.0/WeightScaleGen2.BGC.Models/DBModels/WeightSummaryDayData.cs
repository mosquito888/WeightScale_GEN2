namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class WeightSummaryDayData
    {
        public string item_code { get; set; }
        public string item_name { get; set; }
        public int supplier_code { get; set; }
        public string supplier_name { get; set; }
        public int weight_count { get; set; }
        public decimal sum_weight_out { get; set; }
        public string group_name { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
