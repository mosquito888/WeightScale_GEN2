using System;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.MMPO
{
    public class ParamSearchMMPOViewModel : ParamJqueryDataTable
    {
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
}
