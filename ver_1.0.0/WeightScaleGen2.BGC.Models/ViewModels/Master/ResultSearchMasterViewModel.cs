namespace WeightScaleGen2.BGC.Models.ViewModels.Master
{
    public class ResultSearchMasterViewModel
    {
        public string comp_code { get; set; }
        public string plant_code { get; set; }
        public string master_type { get; set; }
        public string master_type_desc { get; set; }
        public string master_code { get; set; }
        public string master_value1 { get; set; }
        public string master_value2 { get; set; }
        public string master_value3 { get; set; }
        public string master_desc_th { get; set; }
        public string master_desc_en { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public bool is_all { get; set; }
        public string is_add { get; set; }
        public string is_not_edit { get; set; }
        public string is_not_del { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        public string role_name { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
