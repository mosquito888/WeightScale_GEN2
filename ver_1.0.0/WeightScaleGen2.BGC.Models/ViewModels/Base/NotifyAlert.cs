using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ViewModels.Base
{
    public class NotifyAlert
    {
        public NotifyAlert()
        {
            data = new List<NotifyAlertList>();
        }
        public List<NotifyAlertList> data { get; set; }
        public int total { get; set; }
    }

    public class NotifyAlertList
    {
        public string url { get; set; }
        public string status { get; set; }
        public string text { get; set; }
        public string id { get; set; }
    }
}
