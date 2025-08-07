using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class Authorize
{
    public int Id { get; set; }

    public int AuLevel { get; set; }

    public string Description { get; set; }

    public string Remark { get; set; }
}
