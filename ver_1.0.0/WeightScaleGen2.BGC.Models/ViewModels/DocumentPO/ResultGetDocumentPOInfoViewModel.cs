using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.DocumentPO
{
    public class ResultGetDocumentPOViewModel
    {
        public string purchase_number { get; set; }
        public int num_of_rec { get; set; }
        public string company_code { get; set; }
        public string plant { get; set; }
        public string storage_loc { get; set; }
        public string status { get; set; }
        public string vender_code { get; set; }
        public string vender_name { get; set; }
        public string material_code { get; set; }
        public string material_desc { get; set; }
        public decimal order_qty { get; set; }
        public string uom { get; set; }
        public string uom_in { get; set; }
        public decimal good_received { get; set; }
        public decimal pending_qty { get; set; }
        public decimal pending_qty_all { get; set; }
        public decimal allowance { get; set; }
        public string dlv_complete { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public TimeSpan created_time { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public TimeSpan modified_time { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }

    public class ResultGetDocumentPOInfoViewModel
    {
        public string purchase_number { get; set; }
        public int num_of_rec { get; set; }
        public string company_code { get; set; }
        public string plant { get; set; }
        public string storage_loc { get; set; }
        public string status { get; set; }
        public string vender_code { get; set; }
        public string vender_name { get; set; }
        public string material_code { get; set; }
        public string material_desc { get; set; }
        public decimal order_qty { get; set; }
        public string uom { get; set; }
        public string uom_in { get; set; }
        public decimal good_received { get; set; }
        public decimal pending_qty { get; set; }
        public decimal pending_qty_all { get; set; }
        public decimal allowance { get; set; }
        public string dlv_complete { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public TimeSpan created_time { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public TimeSpan modified_time { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }
}
