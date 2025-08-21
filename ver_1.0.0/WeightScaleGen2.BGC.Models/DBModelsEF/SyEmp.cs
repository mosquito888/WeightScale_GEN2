using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyEmp
{
    public string EmpCode { get; set; }

    public string CompCode { get; set; }

    public string PlantCode { get; set; }

    public string PlantTemp { get; set; }

    public string DeptCode { get; set; }

    public string DeptTemp { get; set; }

    public string Name { get; set; }

    public string PayType { get; set; }

    public DateOnly? WorkStartDate { get; set; }

    public string AddrLine1 { get; set; }

    public string AddrLine2 { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string ImgName { get; set; }

    public string ImgType { get; set; }

    public byte[] ImgByte { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }
}
