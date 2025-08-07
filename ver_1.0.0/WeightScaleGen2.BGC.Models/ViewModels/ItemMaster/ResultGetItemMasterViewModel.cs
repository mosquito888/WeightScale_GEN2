using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.ItemMaster
{
    public class ResultGetItemMasterViewModel
    {
        public int item_shot { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_group { get; set; }
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

    public class ResultGetItemMasterInfoViewModel
    {
        public string mode { get; set; }
        public int item_shot { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_group { get; set; }
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
