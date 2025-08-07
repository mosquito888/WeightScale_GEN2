using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightOutHistory
{
    public class ResultSearchWeightOutHistoryViewModel
    {
        public int id { get; set; }
        public string weight_out_no { get; set; }
        public string weight_out_type { get; set; }
        public string car_license { get; set; }
        public string weight_in_no { get; set; }
        public string base_unit { get; set; }
        public string unit_receive { get; set; }
        public decimal gross_uom { get; set; }
        public decimal net_uom { get; set; }
        public string status { get; set; }
        public DateTime? date { get; set; }
        public decimal before_weight_out { get; set; }
        public decimal weight_out { get; set; }
        public decimal weight_receive { get; set; }
        public decimal percent_humidity_out { get; set; }
        public decimal percent_humidity_ok { get; set; }
        public decimal percent_humidity_diff { get; set; }
        public decimal weight_bag { get; set; }
        public decimal qty_bag { get; set; }
        public decimal total_weight_bag { get; set; }
        public decimal weight_pallet { get; set; }
        public decimal qty_pallet { get; set; }
        public decimal total_weight_pallet { get; set; }
        public decimal weight_by_supplier { get; set; }
        public decimal volume_by_supplier { get; set; }
        public decimal sg_supplier { get; set; }
        public decimal sg_bg { get; set; }
        public decimal api_supplier { get; set; }
        public decimal api_bg { get; set; }
        public decimal temp_supplier { get; set; }
        public decimal temp_bg { get; set; }
        public string remark_1 { get; set; }
        public string remark_2 { get; set; }
        public string user_id { get; set; }
        public string user_edit_1 { get; set; }
        public string user_edit_2 { get; set; }
        public string user_edit_3 { get; set; }
        public int reprint { get; set; }
        public string company { get; set; }
        public string maintenance_no { get; set; }
        public int edi { get; set; }
        public string edi_send { get; set; }
    }
}
