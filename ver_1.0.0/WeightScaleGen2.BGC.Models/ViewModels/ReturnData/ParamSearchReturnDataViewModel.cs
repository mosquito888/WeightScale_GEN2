using System;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.ReturnData
{
    public class ParamSearchReturnDataViewModel : ParamJqueryDataTable
    {
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public bool status { get; set; }
        // Option Send Data to SAP
        public string user { get; set; }
        public string password { get; set; }
    }
}
