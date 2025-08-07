using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class MaintenanceWeightIn
{
    public string MaintenanceNo { get; set; }

    public string WeightInNo { get; set; }

    public string WeightInType { get; set; }

    public decimal? LineNumber { get; set; }

    public decimal? LineNumberE { get; set; }

    public string ItemCode { get; set; }

    public string ItemCodeE { get; set; }

    public string ItemName { get; set; }

    public string ItemNameE { get; set; }

    public int SupplierCode { get; set; }

    public int? SupplierCodeE { get; set; }

    public string CarLicense { get; set; }

    public string CarLicenseE { get; set; }

    public string CarType { get; set; }

    public string CarTypeE { get; set; }

    public string DocumentPo { get; set; }

    public string DocumentPoE { get; set; }

    public string DocTypePo { get; set; }

    public string DocTypePoE { get; set; }

    public string DocumentRef { get; set; }

    public string DocumentRefE { get; set; }

    public decimal WeightIn { get; set; }

    public decimal? WeightInE { get; set; }

    public DateTime Date { get; set; }

    public DateTime? DateE { get; set; }

    public string UserId { get; set; }

    public string UserIdE { get; set; }

    public string Status { get; set; }

    public string StatusE { get; set; }

    public string Company { get; set; }

    public string CompanyE { get; set; }

    public string UserCreate { get; set; }

    public string UserEdit { get; set; }

    public string UserApprove { get; set; }

    public string Remark1 { get; set; }

    public string Remark2 { get; set; }

    public DateTime? DocStart { get; set; }

    public DateTime? DocStartE { get; set; }

    public DateTime? DocStop { get; set; }

    public DateTime? DocStopE { get; set; }

    public string DocSend { get; set; }

    public string DocSendE { get; set; }

    public DateTime DateCreate { get; set; }

    public DateTime? DateEdit { get; set; }

    public DateTime? DateApprove { get; set; }

    public int ApproveLevel { get; set; }

    public int Reason { get; set; }

    public string Note { get; set; }

    public string StatusMaintenance { get; set; }
}
