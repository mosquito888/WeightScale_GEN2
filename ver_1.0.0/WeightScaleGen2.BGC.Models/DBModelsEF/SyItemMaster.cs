using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyItemMaster
{
    public int? ItemShot { get; set; }

    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    public string ItemGroup { get; set; }

    public string Status { get; set; }

    public string Remark1 { get; set; }

    public string Remark2 { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }
}
