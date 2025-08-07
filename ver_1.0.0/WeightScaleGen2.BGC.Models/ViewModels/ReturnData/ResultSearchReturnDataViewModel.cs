using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.ReturnData
{
    public class ResultSearchReturnDataViewModel
    {
        public string weight_out_no { get; set; }
        public string weight_in_no { get; set; }
        public decimal sequence { get; set; }
        public decimal gr_type { get; set; }
        public DateTime doc_date { get; set; }
        public DateTime post_date { get; set; }
        public string ref_doc { get; set; }
        public string good_movement { get; set; }
        public string material { get; set; }
        public string plant { get; set; }
        public string sloc { get; set; }
        public string stock_type { get; set; }
        public string item_text { get; set; }
        public string po_number { get; set; }
        public decimal po_line_number { get; set; }
        public string truck_no { get; set; }
        public decimal weight_in { get; set; }
        public decimal weight_out { get; set; }
        public decimal weight_rec { get; set; }
        public decimal weight_vendor { get; set; }
        public decimal weight_reject { get; set; }
        public string weight_unit { get; set; }
        public DateTime? doc_start { get; set; }
        public DateTime? doc_stop { get; set; }
        public string doc_send { get; set; }
        public string message_type { get; set; }
        public string message { get; set; }
        public string send_data { get; set; }
        public string material_document { get; set; }
        public decimal document_year { get; set; }
        #region [ADD ON]
        public int total_record { get; set; }
        #endregion [ADD ON]
    }
}
