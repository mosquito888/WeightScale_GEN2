using System;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class UOMConversionSapData
    {
        public string MaterialCode { get; set; }
        public string AlterUOM { get; set; }
        public string BaseUOM { get; set; }
        public string AlterUOM_IN { get; set; }
        public string BaseUOM_IN { get; set; }
        public decimal ConvWeightN { get; set; }
        public decimal ConvWeightD { get; set; }
        public decimal NetWeight { get; set; }
        public decimal GrossWeight { get; set; }
        public string WeightUnit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public TimeSpan CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public TimeSpan UpdatedTime { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
