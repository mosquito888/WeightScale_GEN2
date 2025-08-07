using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Sender
{
    public class ParamSearchSenderViewModel : ParamJqueryDataTable
    {
        public int id { get; set; }
        public string sender_name { get; set; }
    }

    public class ParamSenderInfo
    {
        public int id { get; set; }
    }
}
