using System;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.DocumentPO
{
    public class ParamSearchDocumentPOViewModel : ParamJqueryDataTable
    {
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
}
