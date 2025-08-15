using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SySenderMapping
{
    public int Id { get; set; }

    public string WeightInNo { get; set; }

    public int SenderId { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }
}
