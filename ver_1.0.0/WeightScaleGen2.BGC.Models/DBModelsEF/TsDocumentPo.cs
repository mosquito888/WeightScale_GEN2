using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class TsDocumentPo
{
    public string PurchaseNumber { get; set; }

    public decimal NumOfRec { get; set; }

    public string CompanyCode { get; set; }

    public string Plant { get; set; }

    public string StorageLoc { get; set; }

    public string Status { get; set; }

    public string VenderCode { get; set; }

    public string VenderName { get; set; }

    public string MaterialCode { get; set; }

    public string MaterialDesc { get; set; }

    public decimal? OrderQty { get; set; }

    public string Uom { get; set; }

    public string UomIn { get; set; }

    public decimal? GoodReceived { get; set; }

    public decimal? PendingQty { get; set; }

    public decimal? PendingQtyAll { get; set; }

    public decimal? Allowance { get; set; }

    public string DlvComplete { get; set; }

    public string CreatedBy { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public TimeOnly? CreatedTime { get; set; }

    public string ModifiedBy { get; set; }

    public DateOnly? ModifiedDate { get; set; }

    public TimeOnly? ModifiedTime { get; set; }
}
