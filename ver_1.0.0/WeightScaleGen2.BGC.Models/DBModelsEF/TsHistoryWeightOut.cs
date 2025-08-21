using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class TsHistoryWeightOut
{
    public int Id { get; set; }

    public string WeightOutNo { get; set; }

    public string WeightOutType { get; set; }

    public string CarLicense { get; set; }

    public string WeightInNo { get; set; }

    public string BaseUnit { get; set; }

    public string UnitReceive { get; set; }

    public decimal? GrossUom { get; set; }

    public decimal? NetUom { get; set; }

    public string Status { get; set; }

    public DateTime Date { get; set; }

    public decimal? BeforeWeightOut { get; set; }

    public decimal? WeightOut { get; set; }

    public decimal? WeightReceive { get; set; }

    public decimal? PercentHumidityOut { get; set; }

    public decimal? PercentHumidityOk { get; set; }

    public decimal? PercentHumidityDiff { get; set; }

    public decimal? WeightBag { get; set; }

    public decimal? QtyBag { get; set; }

    public decimal? TotalWeightBag { get; set; }

    public decimal? WeightPallet { get; set; }

    public decimal? QtyPallet { get; set; }

    public decimal? TotalWeightPallet { get; set; }

    public decimal? WeightBySupplier { get; set; }

    public decimal? VolumeBySupplier { get; set; }

    public decimal? SgSupplier { get; set; }

    public decimal? SgBg { get; set; }

    public decimal? ApiSupplier { get; set; }

    public decimal? ApiBg { get; set; }

    public decimal? TempSupplier { get; set; }

    public decimal? TempBg { get; set; }

    public string Remark1 { get; set; }

    public string Remark2 { get; set; }

    public string UserId { get; set; }

    public string UserEdit1 { get; set; }

    public string UserEdit2 { get; set; }

    public string UserEdit3 { get; set; }

    public int? Reprint { get; set; }

    public string Company { get; set; }

    public string MaintenanceNo { get; set; }

    public int? Edi { get; set; }

    public string EdiSend { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }
}
