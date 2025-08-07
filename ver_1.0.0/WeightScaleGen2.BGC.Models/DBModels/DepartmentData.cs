using System;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class DepartmentData
    {
        public string dept_code { get; set; }
        public string comp_code { get; set; }
        public string plant_code { get; set; }
        public string short_code { get; set; }
        public string name_th { get; set; }
        public string name_en { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public bool is_all { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        public string role_name { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
