using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightHistory
{
    public class ResultSearchWeightHistoryViewModel
    {
        public int id { get; set; }
        public string weight_out_no { get; set; }
        public string weight_in_no { get; set; }
        public decimal before_weight_out { get; set; }
        public decimal weight_in { get; set; }
        public decimal weight_receive { get; set; }
        public int supplier_code { get; set; }
        public string supplier_name { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_remark { get; set; }
        public string weight_out_type { get; set; }
        public string car_license { get; set; }
        public DateTime? weight_out_date { get; set; }
        public string company { get; set; }
        public string user_edit_1 { get; set; }
        public string user_edit_2 { get; set; }
        public string user_edit_3 { get; set; }
        public decimal sg_bg { get; set; }
        public decimal sg_supplier { get; set; }
        public decimal api_bg { get; set; }
        public decimal api_supplier { get; set; }
        public decimal temp_bg { get; set; }
        public decimal temp_supplier { get; set; }
        public string remark_1 { get; set; }
        public string remark_2 { get; set; }
        public string document_def { get; set; }
        public DateTime? weight_in_date { get; set; }
        public string user_id { get; set; }
        public string document_po { get; set; }
        public string doc_type_po { get; set; }
        public decimal weight_by_supplier { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
