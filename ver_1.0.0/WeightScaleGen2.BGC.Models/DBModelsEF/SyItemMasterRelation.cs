using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyItemMasterRelation
{
    public int Id { get; set; }

    public int? SupplierCode { get; set; }

    public string ItemCode { get; set; }

    public decimal? Humidity { get; set; }

    public decimal? Gravity { get; set; }

    public string Status { get; set; }

    public string Remark1 { get; set; }

    public string Remark2 { get; set; }
}
