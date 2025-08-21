using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SySystem
{
    public string SysType { get; set; }

    public string SysCode { get; set; }

    public string SysValue { get; set; }

    public string SysDesc { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }
}
