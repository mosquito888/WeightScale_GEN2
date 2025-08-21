using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyMasterType
{
    public string MasterType { get; set; }

    public string MasterTypeDesc { get; set; }

    public bool IsAdd { get; set; }

    public bool IsNotEdit { get; set; }

    public bool IsNotDel { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }

    public virtual ICollection<SyMaster> SyMasters { get; set; } = new List<SyMaster>();
}
