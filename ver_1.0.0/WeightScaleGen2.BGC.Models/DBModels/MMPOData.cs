using System;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class MMPOData
    {
        public string PurchaseNumber { get; set; }
        public int NumOfRec { get; set; }
        public string CompanyCode { get; set; }
        public string Plant { get; set; }
        public string StorageLoc { get; set; }
        public string Status { get; set; }
        public string VenderCode { get; set; }
        public string VenderName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public decimal OrderQty { get; set; }
        public string UOM { get; set; }
        public string UOM_IN { get; set; }
        public decimal GoodReceived { get; set; }
        public decimal PendingQty { get; set; }
        public decimal PendingQtyAll { get; set; }
        public decimal Allowance { get; set; }
        public string DlvComplete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public TimeSpan CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public TimeSpan UpdatedTime { get; set; }
        #region [ADD ON{ get; set; }]
        public int total_record { get; set; }
        #endregion [ADD ON{ get; set; }]
    }
}
