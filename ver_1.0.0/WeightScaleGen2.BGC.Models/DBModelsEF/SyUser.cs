using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyUser
{
    public long UserId { get; set; }

    public long RoleId { get; set; }

    public string EmpCode { get; set; }

    public string Name { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }

    public virtual SyRole Role { get; set; }
}
