using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.UOMConversion
{
    public class ResultSearchUOMConversionViewModel
    {
        public string material_code { get; set; }
        public string alter_uom { get; set; }
        public string base_uom { get; set; }
        public string alter_uom_in { get; set; }
        public string base_uom_in { get; set; }
        public decimal conv_weight_n { get; set; }
        public decimal conv_weight_d { get; set; }
        public decimal net_weight { get; set; }
        public decimal gross_weight { get; set; }
        public string weight_unit { get; set; }
        public string created_by { get; set; }
        public DateTime created_on { get; set; }
        public TimeSpan created_time { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_on { get; set; }
        public TimeSpan updated_time { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
