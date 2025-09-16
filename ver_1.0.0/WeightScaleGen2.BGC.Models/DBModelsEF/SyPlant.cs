using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyPlant
{
    public string PlantCode { get; set; }

    public string CompCode { get; set; }

    public string ShortCode { get; set; }

    public string ShortPlantCode { get; set; }

    public string ProvinceCode { get; set; }

    public string NameTh { get; set; }

    public string NameEn { get; set; }

    public string AddrThLine1 { get; set; }

    public string AddrThLine2 { get; set; }

    public string AddrEnLine1 { get; set; }

    public string AddrEnLine2 { get; set; }

    public string HeadReport { get; set; }

    public string ReportType { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public string SerialPort { get; set; }
}
