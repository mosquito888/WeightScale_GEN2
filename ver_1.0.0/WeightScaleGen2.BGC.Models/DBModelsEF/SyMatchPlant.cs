using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyMatchPlant
{
    public long Id { get; set; }

    public string CompCode { get; set; }

    public string PlantCode { get; set; }

    public string MatchPlantCode { get; set; }

    public string MatchCompCode { get; set; }
}
