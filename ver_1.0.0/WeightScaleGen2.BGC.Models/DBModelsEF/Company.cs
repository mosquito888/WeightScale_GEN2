using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class Company
{
    public string CompanyCode { get; set; }

    public string CompanyName { get; set; }

    public string CompanyNameEn { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string Address3 { get; set; }

    public string Address1En { get; set; }

    public string Address2En { get; set; }

    public string Address3En { get; set; }

    public string Status { get; set; }

    public string Remark1 { get; set; }

    public string Remark2 { get; set; }

    public string DigitSlip { get; set; }
}
