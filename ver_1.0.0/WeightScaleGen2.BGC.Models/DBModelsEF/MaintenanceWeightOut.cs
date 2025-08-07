using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class MaintenanceWeightOut
{
    public string MaintenanceNo { get; set; }

    public string WeightOutNo { get; set; }

    public string WeightOutType { get; set; }

    public string WeightOutTypeE { get; set; }

    public string CarLicense { get; set; }

    public string CarLicenseE { get; set; }

    public string WeightInNo { get; set; }

    public string WeightInNoE { get; set; }

    public string Status { get; set; }

    public string StatusE { get; set; }

    public DateTime Date { get; set; }

    public DateTime? DateE { get; set; }

    public decimal? BeforeWeightOut { get; set; }

    public decimal? BeforeWeightOutE { get; set; }

    public decimal? WeightOut { get; set; }

    public decimal? WeightOutE { get; set; }

    public decimal? WeightReceive { get; set; }

    public decimal? WeightReceiveE { get; set; }

    public decimal? PercentHumidityOut { get; set; }

    public decimal? PercentHumidityOutE { get; set; }

    public decimal? PercentHumidityOk { get; set; }

    public decimal? PercentHumidityDiff { get; set; }

    public decimal? PercentHumidityDiffE { get; set; }

    public decimal? WeightBag { get; set; }

    public decimal? WeightBagE { get; set; }

    public decimal? QtyBag { get; set; }

    public decimal? QtyBagE { get; set; }

    public decimal? TotalWeightBag { get; set; }

    public decimal? TotalWeightBagE { get; set; }

    public decimal? WeightPallet { get; set; }

    public decimal? WeightPalletE { get; set; }

    public decimal? QtyPallet { get; set; }

    public decimal? QtyPalletE { get; set; }

    public decimal? TotalWeightPallet { get; set; }

    public decimal? TotalWeightPalletE { get; set; }

    public decimal? WeightBySupplier { get; set; }

    public decimal? WeightBySupplierE { get; set; }

    public decimal? VolumeBySupplier { get; set; }

    public decimal? VolumeBySupplierE { get; set; }

    public decimal? SgSupplier { get; set; }

    public decimal? SgSupplierE { get; set; }

    public decimal? SgBg { get; set; }

    public decimal? SgBgE { get; set; }

    public decimal? ApiSupplier { get; set; }

    public decimal? ApiSupplierE { get; set; }

    public decimal? ApiBg { get; set; }

    public decimal? ApiBgE { get; set; }

    public decimal? TempSupplier { get; set; }

    public decimal? TempSupplierE { get; set; }

    public decimal? TempBg { get; set; }

    public decimal? TempBgE { get; set; }

    public string Remark1 { get; set; }

    public string Remark1E { get; set; }

    public string Remark2 { get; set; }

    public string Remark2E { get; set; }

    public string Company { get; set; }

    public string CompanyE { get; set; }

    public string UserId { get; set; }

    public string UserIdE { get; set; }

    public string UserCreate { get; set; }

    public string UserEdit { get; set; }

    public string UserApprove { get; set; }

    public DateTime? DateCreate { get; set; }

    public DateTime? DateEdit { get; set; }

    public DateTime? DateApprove { get; set; }

    public int ApproveLevel { get; set; }

    public int? Reason { get; set; }

    public string Note { get; set; }

    public string StatusMaintenance { get; set; }
}
