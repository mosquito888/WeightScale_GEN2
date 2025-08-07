using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class UserLog
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public DateTime? DateIn { get; set; }

    public DateTime? DateOut { get; set; }

    public DateTime? Date { get; set; }

    public string CompanyCode { get; set; }

    public string Status { get; set; }

    public string Remark1 { get; set; }

    public string Remark2 { get; set; }
}
