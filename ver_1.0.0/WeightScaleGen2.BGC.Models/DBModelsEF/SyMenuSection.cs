using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyMenuSection
{
    public long MenuSectionId { get; set; }

    public long MenuId { get; set; }

    public long ListNo { get; set; }

    public string MenuSectionName { get; set; }

    public string MenuSectionNameDisplay { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<SyRoleItem> SyRoleItems { get; set; } = new List<SyRoleItem>();
}
