using System;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class WeightInHistoryData
    {
        public int id { get; set; }
        public string weight_in_no { get; set; }
        public string weight_in_type { get; set; }
        public int line_number { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public int supplier_code { get; set; }
        public string car_license { get; set; }
        public string car_type { get; set; }
        public string document_po { get; set; }
        public string doc_type_po { get; set; }
        public string document_ref { get; set; }
        public decimal weight_in { get; set; }
        public DateTime? date { get; set; }
        public string user_id { get; set; }
        public string status { get; set; }
        public string user_edit_1 { get; set; }
        public string user_edit_2 { get; set; }
        public string user_edit_3 { get; set; }
        public string remark_1 { get; set; }
        public string remark_2 { get; set; }
        public int reprint { get; set; }
        public string company { get; set; }
        public string maintenance_no { get; set; }
        public DateTime? doc_start { get; set; }
        public DateTime? doc_stop { get; set; }
        public string doc_send { get; set; }
        public int edi { get; set; }
        public int edi_sand { get; set; }
        public int sender_id { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
