namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class MasterData
    {
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
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        public string role_name { get; set; }
        #endregion [ADD ON{ get; set; }]
    }

    public class MasterDataType
    {
        public string master_type { get; set; }
        public string master_type_desc { get; set; }
        public bool is_add { get; set; }
        public bool is_not_edit { get; set; }
        public bool is_not_del { get; set; }
    }
}
