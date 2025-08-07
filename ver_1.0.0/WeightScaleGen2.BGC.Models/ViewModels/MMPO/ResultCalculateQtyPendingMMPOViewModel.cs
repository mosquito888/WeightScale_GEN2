using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.MMPO
{
    public class ResultCalculateQtyPendingMMPOViewModel
    {
        public decimal po_order { get; set; }
        public decimal po_receive { get; set; }
        public decimal po_pending { get; set; }
        public decimal po_pending_all { get; set; }
        public DateTime update_time { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public DateTime last_update { get; set; }
        public decimal allowance { get; set; }
        public string uom { get; set; }
        public string sta { get; set; }
        public string deliver { get; set; }
        public decimal num_of_rec { get; set; }
    }
}
