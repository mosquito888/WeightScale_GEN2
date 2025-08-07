using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.Supplier
{
    public class ResultGetSupplierViewModel
    {
        public int supplier_code { get; set; }
        public string supplier_name { get; set; }
        public string status { get; set; }
        public string remark_1 { get; set; }
        public string remark_2 { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }

    public class ResultGetSupplierInfoViewModel
    {
        public string mode { get; set; }
        public int supplier_code { get; set; }
        public string supplier_name { get; set; }
        public string status { get; set; }
        public string remark_1 { get; set; }
        public string remark_2 { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }
}
