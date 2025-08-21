using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class TsIdentNumber
{
    public string Company { get; set; }

    public string Year { get; set; }

    public string Month { get; set; }

    public int? IdenNumber { get; set; }

    public string Type { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }
}
