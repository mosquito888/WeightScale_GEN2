using System.Collections.Generic;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.User
{
    public class ResultGetUserInfo
    {
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string role_id { get; set; }
        public string role_name { get; set; }
        public string user_id { get; set; }
        public string emp_code { get; set; }
        public string serial_port { get; set; }
        public IEnumerable<BaseDLLViewModel> role_dll { get; set; }
        public ResultGetUserInfo()
        {
            role_dll = new List<BaseDLLViewModel>();
        }
    }
}
