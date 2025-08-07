using System;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.WeightDaily
{
    public class ParamSearchWeightDailyViewModel : ParamJqueryDataTable
    {
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool close_work { get; set; }
    }
}
