namespace WeightScaleGen2.BGC.Models.ViewModels.Master
{
    public class ResultGetMasterViewModel
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
    }

    public class ResultGetMasterTypeViewModel
    {
        public string master_type { get; set; }
        public string master_type_desc { get; set; }
        public bool is_add { get; set; }
        public bool is_not_edit { get; set; }
        public bool is_not_del { get; set; }
    }

    public class ResultGetMasterInfoViewModel
    {
        public string mode { get; set; }
        public string comp_code { get; set; }
        public string plant_code { get; set; }
        public string master_type { get; set; }
        public string master_code { get; set; }
        public string master_value1 { get; set; }
        public string master_value2 { get; set; }
        public string master_value3 { get; set; }
        public string master_desc_th { get; set; }
        public string master_desc_en { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public bool is_all { get; set; }
    }
}
