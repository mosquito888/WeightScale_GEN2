using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyRoleItem
{
    public long RoleItemId { get; set; }

    public long RoleId { get; set; }

    public long MenuSectionId { get; set; }

    public bool IsDisplay { get; set; }

    public bool IsAction { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual SyMenuSection MenuSection { get; set; }

    public virtual SyRole Role { get; set; }
}
