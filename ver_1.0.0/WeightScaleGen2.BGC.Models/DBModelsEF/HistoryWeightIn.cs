using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class HistoryWeightIn
{
    public int Id { get; set; }

    public string WeightInNo { get; set; }

    public string WeightInType { get; set; }

    public decimal? LineNumber { get; set; }

    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    public int? SupplierCode { get; set; }

    public string CarLicense { get; set; }

    public string CarType { get; set; }

    public string DocumentPo { get; set; }

    public string DocTypePo { get; set; }

    public string DocumentRef { get; set; }

    public decimal? WeightIn { get; set; }

    public DateTime? Date { get; set; }

    public string UserId { get; set; }

    public string Status { get; set; }

    public string UserEdit1 { get; set; }

    public string UserEdit2 { get; set; }

    public string UserEdit3 { get; set; }

    public string Remark1 { get; set; }

    public string Remark2 { get; set; }

    public int? Reprint { get; set; }

    public string Company { get; set; }

    public string MaintenanceNo { get; set; }

    public DateTime? DocStart { get; set; }

    public DateTime? DocStop { get; set; }

    public string DocSend { get; set; }

    public int? Edi { get; set; }

    public string EdiSand { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }
}
