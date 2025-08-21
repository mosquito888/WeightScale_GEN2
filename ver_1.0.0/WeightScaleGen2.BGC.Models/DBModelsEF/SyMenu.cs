using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class SyMenu
{
    public long MenuId { get; set; }

    public int MenuLevel { get; set; }

    public int? ParentMenuId { get; set; }

    public int ListNo { get; set; }

    public string MenuName { get; set; }

    public string DisplayName { get; set; }

    public long MenuDefinition { get; set; }

    public string Icon { get; set; }

    public string UrlController { get; set; }

    public string Url { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }
}
