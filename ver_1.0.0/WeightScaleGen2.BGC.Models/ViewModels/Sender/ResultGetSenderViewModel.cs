using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.Sender
{
    public class ResultGetSenderViewModel
    {
        public int id { get; set; }
        public string sender_name { get; set; }
        public string flag_delete { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }

    public class ResultGetSenderInfoViewModel
    {
        public string mode { get; set; }
        public int id { get; set; }
        public string sender_name { get; set; }
        public string flag_delete { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }
}
