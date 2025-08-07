using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class UomConversion
{
    public string MaterialCode { get; set; }

    public string AlterUom { get; set; }

    public string BaseUom { get; set; }

    public string AlterUomIn { get; set; }

    public string BaseUomIn { get; set; }

    public decimal? ConvWeightN { get; set; }

    public decimal? ConvWeightD { get; set; }

    public decimal? NetWeight { get; set; }

    public decimal? GrossWeight { get; set; }

    public string WeightUnit { get; set; }

    public string CreatedBy { get; set; }

    public DateOnly? CreatedOn { get; set; }

    public TimeOnly? CreatedTime { get; set; }

    public string UpdatedBy { get; set; }

    public DateOnly? UpdatedOn { get; set; }

    public TimeOnly? UpdatedTime { get; set; }
}
