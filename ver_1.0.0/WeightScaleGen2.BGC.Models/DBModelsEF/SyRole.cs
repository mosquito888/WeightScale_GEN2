using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyRole
{
    public long RoleId { get; set; }

    public string RoleName { get; set; }

    public string RoleDesc { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsSuperRole { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }

    public virtual ICollection<SyRoleItem> SyRoleItems { get; set; } = new List<SyRoleItem>();

    public virtual ICollection<SyUser> SyUsers { get; set; } = new List<SyUser>();
}
