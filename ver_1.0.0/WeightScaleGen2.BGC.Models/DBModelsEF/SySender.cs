using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SySender
{
    public int Id { get; set; }

    public string SenderName { get; set; }

    public string FlagDelete { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }
}
