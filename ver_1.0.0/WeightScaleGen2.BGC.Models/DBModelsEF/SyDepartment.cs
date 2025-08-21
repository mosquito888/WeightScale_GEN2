using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyDepartment
{
    public string DeptCode { get; set; }

    public string CompCode { get; set; }

    public string PlantCode { get; set; }

    public string ShortCode { get; set; }

    public string NameTh { get; set; }

    public string NameEn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsAll { get; set; }
}
