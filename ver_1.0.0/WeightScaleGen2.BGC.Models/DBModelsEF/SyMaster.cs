using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyMaster
{
    public string CompCode { get; set; }

    public string PlantCode { get; set; }

    public string MasterType { get; set; }

    public string MasterCode { get; set; }

    public string MasterValue1 { get; set; }

    public string MasterValue2 { get; set; }

    public string MasterValue3 { get; set; }

    public string MasterDescTh { get; set; }

    public string MasterDescEn { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsAll { get; set; }

    public virtual SyMasterType MasterTypeNavigation { get; set; }
}
