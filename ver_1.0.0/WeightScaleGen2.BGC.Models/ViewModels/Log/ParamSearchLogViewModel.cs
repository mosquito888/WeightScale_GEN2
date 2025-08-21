using System;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Log
{
    public class ParamSearchLogViewModel : ParamJqueryDataTable
    {
        public string username { get; set; }
        public string level { get; set; }
        public DateTime? logDataFrom { get; set; }
        public DateTime? logDataTo { get; set; }
    }
}
