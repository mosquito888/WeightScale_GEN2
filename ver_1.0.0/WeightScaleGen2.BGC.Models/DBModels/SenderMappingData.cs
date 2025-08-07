namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class SenderMappingData
    {
        public int id { get; set; }
        public string weight_in_no { get; set; }
        public int sender_id { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
