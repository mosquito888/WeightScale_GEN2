using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.DBModelsEF;

public partial class TsReturnDatum
{
    public string WeightOutNo { get; set; }

    public string WeightInNo { get; set; }

    public decimal Sequence { get; set; }

    public string GrType { get; set; }

    public DateOnly DocDate { get; set; }

    public DateOnly PostDate { get; set; }

    public string RefDoc { get; set; }

    public string GoodMovement { get; set; }

    public string Material { get; set; }

    public string Plant { get; set; }

    public string Sloc { get; set; }

    public string StockType { get; set; }

    public string ItemText { get; set; }

    public string PoNumber { get; set; }

    public decimal PoLineNumber { get; set; }

    public string TruckNo { get; set; }

    public decimal WeightIn { get; set; }

    public decimal WeightOut { get; set; }

    public decimal WeightRec { get; set; }

    public decimal WeightVendor { get; set; }

    public decimal? WeightReject { get; set; }

    public string WeightUnit { get; set; }

    public DateTime? DocStart { get; set; }

    public DateTime? DocStop { get; set; }

    public string DocSend { get; set; }

    public string MessageType { get; set; }

    public string Message { get; set; }

    public string SendData { get; set; }

    public string MaterialDocument { get; set; }

    public decimal? DocumentYear { get; set; }

    public string PlantCode { get; set; }

    public string CompCode { get; set; }
}
