using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyPrefixDoc
{
    public long Id { get; set; }

    public string CompCode { get; set; }

    public string PlantCode { get; set; }

    public string DocType { get; set; }

    public int? PrefixYear { get; set; }

    public int? PrefixMonth { get; set; }

    public string PrefixPattern { get; set; }

    public string Digit { get; set; }

    public int? SeqCurrent { get; set; }

    public int? SeqNext { get; set; }
}
