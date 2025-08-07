using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class User
{
    public string UserId { get; set; }

    public string Password { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Department { get; set; }

    public string CompanyCode { get; set; }

    public DateTime? DateRegit { get; set; }

    public string Status { get; set; }

    public string Remark1 { get; set; }

    public string Remark2 { get; set; }
}
