using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class ApprovePo
{
    public int DocumentNumber { get; set; }

    public string DocumentType { get; set; }

    public string UserRequest { get; set; }

    public DateTime DateRequest { get; set; }

    public string UserApprove { get; set; }

    public DateTime? DateApprove { get; set; }

    public string Status { get; set; }
}
