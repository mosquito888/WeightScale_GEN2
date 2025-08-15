using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyComp
{
    public string CompCode { get; set; }

    public string NameThLine1 { get; set; }

    public string NameThLine2 { get; set; }

    public string NameEnLine1 { get; set; }

    public string NameEnLine2 { get; set; }

    public string AddrThLine1 { get; set; }

    public string AddrThLine2 { get; set; }

    public string AddrEnLine1 { get; set; }

    public string AddrEnLine2 { get; set; }

    public string Phone { get; set; }

    public string ProvinceCode { get; set; }

    public string HeadReport { get; set; }

    public string ApproveName { get; set; }

    public bool? EditAfterPrint { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public string DigitSlip { get; set; }

    public string PlantCode { get; set; }
}
