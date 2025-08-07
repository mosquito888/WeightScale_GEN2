using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.Employee
{
    public class ResultSearchEmpViewModel
    {
        public string emp_code { get; set; }
        public string comp_code { get; set; }
        public string plant_code { get; set; }
        public string dept_code { get; set; }
        public string name { get; set; }
        public string pay_type { get; set; }
        public DateOnly? work_start_date { get; set; }
        public string addr_line1 { get; set; }
        public string addr_line2 { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string img_name { get; set; }
        public string img_type { get; set; }
        public byte[] img_byte { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        public string role_name { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
