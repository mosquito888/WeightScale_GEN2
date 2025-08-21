using System.Collections.Generic;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.User
{
    public class ResultSearchUserCriteria
    {
        public IEnumerable<BaseDLLViewModel> role_dll { get; set; }

        public ResultSearchUserCriteria()
        {
            role_dll = new List<BaseDLLViewModel>();
        }
    }
}
